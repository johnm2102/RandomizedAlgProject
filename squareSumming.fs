module SquareSumming
open System
open System.Numerics
open System.Collections.Generic
open HashFunctions
open HashTableChain

type PairData(ky:uint64, valu:int) =
    struct
        member this.key = ky
        member this.value = valu
    end

type Interface_helper =
    abstract member handle: PairData -> Unit
    abstract member res: Unit -> bigint

type SquareSummer (h_f_type:Generic_H_F) = class
    let temp_table = new HashTable(h_f_type)

    interface Interface_helper with
        member this.handle to_handle =
            temp_table.increment(uint64 to_handle.key, int64 to_handle.value)

        member this.res () =
            Array.fold (fun (a: bigint) x -> a + List.fold (fun b (c:H_pairs) -> b + bigint (pown c.value 2)) 0I (List.ofSeq x)) 0I (Array.ofSeq temp_table.get_lsts)
end

type stream_integration(inp : PairData list, sqs: Interface_helper) = class
    member this.run() =
        List.map sqs.handle inp
        |> ignore
        sqs.res()
    end