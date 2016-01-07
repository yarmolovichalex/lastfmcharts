module LastFM

open System
open FSharp.Configuration
open FSharp.Data
open Newtonsoft.Json.Linq

type Settings = AppSettings<"App.config">

type Artist = { Name: string; Listeners: int; Plays: int; }

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

let internal getArtistInfoReq artist =
    let response = Http.Request(
                           Settings.LastfmApi.AbsoluteUri, 
                           query = [ "format", "json";
                                     "api_key", Settings.LastfmKey;
                                     "method", "artist.getInfo";
                                     "artist", artist;
                                     "autocorrect", "1" ],
                           silentHttpErrors = true)
    processResponse response

let internal parseGetArtistInfoResp response =
    let response = JObject.Parse(response)
    let artist = response.["artist"]
    match artist with
    | null ->
        failwith (response.["message"].Value<string>())
    | _ ->
        let name = artist.["name"].Value<string>()
        let listeners = artist.["stats"].["listeners"].Value<int>()
        let plays = artist.["stats"].["playcount"].Value<int>()
        { Name = name; Listeners = listeners; Plays = plays; }

let internal getArtistSuggestionsReq userInput =
    let response = Http.Request(
                           Settings.LastfmApi.AbsoluteUri, 
                           query = [ "format", "json";
                                     "api_key", Settings.LastfmKey;
                                     "method", "artist.search";
                                     "artist", userInput ],
                           silentHttpErrors = true)
    processResponse response
    
let internal parseGetArtistSuggestionsResp response =
    let response = JObject.Parse(response)
    let results = response.["results"]
    match results with
    | null ->
        failwith (response.["message"].Value<string>())
    | results as JToken ->
        let suggestionsArtistsArray = results.["artistmatches"].["artist"].Value<JArray>()
        let suggestionsArtists = seq {
            for suggestionArtist in suggestionsArtistsArray do
                yield suggestionArtist.["name"].Value<string>()
        }
        suggestionsArtists

let internal getTopTracksNamesReq artist =
    let response = Http.Request(
                           Settings.LastfmApi.AbsoluteUri, 
                           query = [ "format", "json";
                                     "api_key", Settings.LastfmKey;
                                     "method", "artist.getTopTracks";
                                     "artist", artist;
                                     "autocorrect", "1" ],
                           silentHttpErrors = true)
    processResponse response

let internal parseGetTopTracksNamesResp response =
    let response = JObject.Parse(response)
    let topTracks = response.["toptracks"]
    match topTracks with
    | null ->
        failwith (response.["message"].Value<string>())
    | topTracks ->
        let tracksArray = topTracks.["track"].Value<JArray>()
        let tracksNamesArray = seq {
            for track in tracksArray do
                yield track.["name"].Value<string>()
        }
        tracksNamesArray

let getArtistInfo artist =
    try
        getArtistInfoReq artist |> parseGetArtistInfoResp
    with 
    | ex -> raise(Exception("Failed to get artist '" + artist + "': " + ex.Message))

let getArtistSuggestions userInput =
    try 
        getArtistSuggestionsReq userInput |> parseGetArtistSuggestionsResp
    with
    | ex -> raise(Exception("Failed to get suggestions for artist '" + userInput + "': " + ex.Message))

let getTopTracksNames artist =
    try
        getTopTracksNamesReq artist |> parseGetTopTracksNamesResp
    with
    | ex -> raise(Exception("Failed to get top tracks for artist '" + artist + "': " + ex.Message))