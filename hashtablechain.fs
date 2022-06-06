module HashTableChain

//creating a hash table 
type Hash_Tab = Table of (uint64 * int) list array * (uint64 -> uint64)
let init size ht =
    let table = [|for i in 1 .. int(2. ** float size) -> [] |]
    Table(table,ht)


//2a. get(x)
let getfunc x (Table (table, ht)) = 
    let rec find x lst = 
        match lst with 
        | [] -> 0 //if x is not in the table then return 0
        | (key, someval) :: xv when x = key -> someval 
        | _ :: xs -> find x xs
    find x table.[int (ht x)] //gets x from table 

//2b. set(x)
let setfunc x v ((Table(table, ht)) as t) = 
    //this function will traverse a list and update a tuple(if it exists)
    let rec update x v lst = 
        match lst with 
        | [] -> [x, v] //adds new tuple to end of the list
        | (key, _) :: table when key = x -> (key, v) :: table 
        | hash :: table -> hash :: update x v table 
    
    ht x //finding right index list
    |> fun index -> table.[int index] <- update x v table.[int index] //this updates the list
    t //this returns the table

