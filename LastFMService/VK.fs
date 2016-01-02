module VK

open System
open FSharp.Configuration
open FSharp.Data
open Newtonsoft.Json.Linq

type Settings = AppSettings<"App.config">

let internal processResponse response = 
    match response.StatusCode with
        | 200 ->
            match response.Body with
            | Text text -> text
            | Binary b -> failwith "Error: not a text"
        | _ ->
            match response.Body with
            | Text text -> failwith text
            | Binary b -> failwith "Error: not a text"

let internal getTrackUrlReq name token =
    let response = Http.Request(
                           Settings.VkAudioSearch.AbsoluteUri, 
                           query = [ "q", name;
                                     "access_token", token; ])
    processResponse response

let internal parseGetTrackUrlResp response =
    let response = JObject.Parse(response)
    let tracks = response.["response"].Value<JArray>()
    match tracks with
    | null ->
        failwith (response.ToString())
    | tracks ->
        // get url of the most relevant track
        tracks.[1].["url"].Value<string>()

let getTrackUrl name token =
    getTrackUrlReq name token |> parseGetTrackUrlResp