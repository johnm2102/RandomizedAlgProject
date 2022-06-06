module HashTableChain
open HashFunctions
open System.Collections.Generic

// creating struct for hash pairs
type H_pairs(ky:uint64, valu:int64) = 
    struct
        member this.key = ky
        member this.value = valu
        override text.ToString() = sprintf "The current H_pair: (%A, %A)" ky valu
    end


//creating a hash table 
type HashTable(h_f : Generic_H_F) =
    let h_f_type = h_f
    let l = h_f_type.l
    let mutable lists = [| for _ in 1L .. (1L <<< l) -> new List<H_pairs>()|]

    member public this.Blocks
        with get() = lists

    member this.get(x:uint64) : Option<int64> =
        let hashed_val = x |> h_f_type.hashed
        let list = lists.[int32 hashed_val]
        let looker = Seq.tryFind(fun (hp: H_pairs) -> hp.key = x) (list)
        match looker with
        | Some pair -> Some pair.value //value if x is in the table
        | None -> None //none if x is not in the table
    
    member this.set(x:uint64, v:int64) =
        let hashed_val = x |> h_f_type.hashed
        let list = lists.[int32 hashed_val]
        let looker = Seq.tryFindIndex(fun (hp: H_pairs) -> hp.key = x) (list)

        match looker with
        | Some pair -> list.[pair] <- H_pairs(x,v) //if x is in the table we change the value to v
        | None -> list.Add(H_pairs(x,v)) //if x is not in the table we add it with value v

    member this.increment(x:uint64, d:int64) =
        let hashed_val = x |> h_f_type.hashed
        let list = lists.[int32 hashed_val]
        let looker = Seq.tryFindIndex(fun (hp: H_pairs) -> hp.key = x) (list)

        match looker with
        | Some pair -> list.[pair] <- (H_pairs(x, list.[pair].value + d)) //if x is in the table, we add d to the value
        | None -> list.Add(H_pairs(x,d)) //if x is not in the table we add is with value d