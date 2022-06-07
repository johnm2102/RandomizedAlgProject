module CountSketch
open System
open System.Numerics
open System.Collections.Generic
open HashFunctions
open SquareSumming

type CountSkether (h_f_type : four_uni_Hashing) = class
    let k_val : int = (1 <<< h_f_type.l)
    let mutable  counts = Array.init k_val (fun x -> 0I)

    interface Interface_helper with
        member this.handle to_handle =
            let h,s = h_f_type.hashed(to_handle.key |> uint64)
            let tmp_val : bigint = (s|> bigint)*(to_handle.value|> bigint)
            counts.[h] <- counts.[h] + tmp_val
        member this.res() =
            Array.fold (fun (a:bigint) x -> a + BigInteger.Pow(x,2)) 0I counts
end