module LastFM

open System
open FSharp.Configuration
open FSharp.Data
open Newtonsoft.Json.Linq

type Settings = AppSettings<"App.config">

//type Track = { Name: string; Number: int; Length: int }
//
//type Album = { Name: string; Tracks: seq<Track> }

type Artist = { Name: string; Listeners: int; Plays: int; Similar: seq<string> }

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
//
//let internal getAlbumInfo artist album =
//    
//    let response = Http.Request(
//                           Settings.LastfmApi.AbsoluteUri, 
//                           query = [ "format", "json";
//                                     "api_key", Settings.LastfmKey;
//                                     "method", "album.getInfo";
//                                     "artist", artist;
//                                     "album", album;
//                                     "autocorrect", "1" ],
//                           silentHttpErrors = true)
//
//    processResponse response
//        
//
//let internal parseGetAlbumInfoResponse response =
//    let response = JsonValue.Parse(response)
//
//    let name = response?album?name.AsString()
//    //let year = DateTime.Parse(response?album?releasedate.AsString()).Year
//
//    let tracksArray = response?album?tracks?track.AsArray()
//    let tracksSeq = seq {
//        for track in tracksArray do
//            yield { Name = track?name.AsString();
//                    Number = track?``@attr``?rank.AsInteger();
//                    Length = track?duration.AsInteger() }
//    }
//
//    {
//        Name = name;
//        //Year = year;
//        Tracks = tracksSeq
//    }
//
let internal getArtistInfo artist =

    let response = Http.Request(
                           Settings.LastfmApi.AbsoluteUri, 
                           query = [ "format", "json";
                                     "api_key", Settings.LastfmKey;
                                     "method", "artist.getInfo";
                                     "artist", artist;
                                     "autocorrect", "1" ],
                           silentHttpErrors = true)

    processResponse response

let internal parseGetArtistInfoResponse response =
    
    let response = JObject.Parse(response)
    
    let artist = response.["artist"]
    match artist with
    | artist as JToken ->  

        let name = artist.["name"].Value<string>()
        let listeners = artist.["stats"].["listeners"].Value<int>()
        let plays = artist.["stats"].["playcount"].Value<int>()

        let similarArray = artist.["similar"].["artist"].Value<JArray>()
        let similarSeq = seq {
            for artist in similarArray do
                yield artist.["name"].Value<string>()
        }

        {
            Name = name;
            Listeners = listeners;
            Plays = plays;
            Similar = similarSeq
        }
    | _ -> failwith (response.["message"].Value<string>())
//
//let getAlbum artist album =
//    getAlbumInfo artist album |> parseGetAlbumInfoResponse
//
let getArtist artist =
    try
        getArtistInfo artist |> parseGetArtistInfoResponse
    with 
    | ex -> raise(Exception("Failed to get artist '" + artist + "': " + ex.Message))
//
//let raiseEx message : unit =
//    raise (Exception(message))
//
//let raiseEx2 message : unit =
//    raise (Exception(message))

//module LastFM
//
//open System
//open FSharp.Configuration
//open FSharp.Data
//open FSharp.Data.JsonExtensions
//
//type Artist = { Name: string; Listeners: int; Plays: int; Similar: seq<string> }
//
//let getArtist artist =
//    {
//        Name = "Metallica";
//        Listeners = 100;
//        Plays = 200;
//        Similar = [ "Megadeth"; "Anthrax" ]
//    }