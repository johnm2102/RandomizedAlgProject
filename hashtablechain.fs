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
//let setfunc xv vv (Table(table, ht)) = 
//    let rec update xv vv lst = 
//        match lst with 
 //       | [] -> [xv, vv] //adds new tuple
