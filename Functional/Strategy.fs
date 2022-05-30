// STRATEGY (POLICY) PATTERN
// Provides the ability to define system behaviour partially at runtime.
// By splitting the algorithm into high-level and low-level parts, the high-level parts can be reused.

let processList items startTok listItem endTok =
    let mid = items |> (Seq.map listItem) |> (String.concat "\n")
    [startTok; mid; endTok] |> String.concat "\n"

let processListMarkdown items =
    processList items "" (fun i -> " * " + i) ""
    
let processListHtml items =
    processList items "<ol>" (fun i -> "  <li>" + i + "</li>") "</ol>" 

[<EntryPoint>]
let main argv =
    let items = ["spam"; "(functional) eggs"; "allan"; "folan"]
    printfn "MARKDOWN LIST%s" (processListMarkdown items)
    printfn "HTML LIST\n%s" (processListHtml items)
    0
