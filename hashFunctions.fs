module HashFunctions
open System.Numerics

type MulShift_Hashing(a:uint64,l:int32) =
    member this.l:int32 = l
    member this.hashed (x:uint64) = ((a*x) >>> (64-l) |> int32)