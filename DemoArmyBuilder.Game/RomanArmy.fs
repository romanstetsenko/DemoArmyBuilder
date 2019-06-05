module RomanArmy

// https://en.wikipedia.org/wiki/Roman_legion

type Unit = 
    | Legionary 
    | Decanus 
    | Contubernium of Unit * (Unit list)
    | Signifer
    | Tesserarius
    | Optio
    | PrimiOrdines
    | Century of (Unit list) * (Unit list)
    | Cohort of Unit list
    | LegatusLegionis
    | Imperial of Unit * (Unit list)


let contubernium = 
    Legionary |> List.replicate 7 //|> fun ls -> Vertical (ls, 1)
    |> fun ls -> Contubernium ( Decanus, ls)

let century =
    let cs = contubernium |> List.replicate 10
    let comm = [PrimiOrdines; Tesserarius; Signifer; Optio]
    Century (comm, cs)

let cohort =
    century |> List.replicate 6
    |> Cohort

let imperial = 
    Imperial (LegatusLegionis, cohort |> List.replicate 9)