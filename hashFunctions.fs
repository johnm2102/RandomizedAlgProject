module HashFunctions
open System.Numerics
open System.Collections.Generic
open System

type Generic_H_F =
    abstract member hashed:uint64 -> int64
    abstract member l:int32

//1a. Implementing mul-shift 
// no bigints as stated in asg-text
type MulShift_Hashing(a:uint64,l:int32) =
    interface Generic_H_F with
        member this.l:int32 = l
        member this.hashed (x:uint64):int64 = ((a*x) >>> (64-l) |> int64)

//1b. Implementing mul-mod-prime-hashing 
// l is less than 64 so int32 should be more than efficient
// I is literal for bigint
// 1<<<89 is same as 2^89, since 1>>1 = 2 and is the same as 2^1 = 2
type MulModPrime_Hashing(a:bigint,b:bigint,l:int32) = 
    let p:bigint = (1I<<<89) - 1I
    interface Generic_H_F with
        member this.l:int32 = l
        member this.hashed (x:uint64):int64 = a*(bigint x)+b |>
            fun x -> (x&&&p) + (x>>>89) |> //from Assignment 3 we know that this is equivalent to x mod p
            fun x -> x&&&(1I<<<l)-1I |> // doing mod 2^l
            fun x -> if x < p then x else x-p // subtracting p if x > p
            |> int64 //piping all the above into an int64 type

type four_uni_Hashing (l:int32,[<ParamArray>]a_s : bigint list) =
    let p:bigint = (1I<<<89) - 1I
    member this.l = l
    member this.hashed (x:uint64):int32*int64 =
        // from Algo 1 in 2nd moment notes
        let mutable y = a_s.[0]
        for i in 1..(a_s.Length-1) do
            y <- y*(bigint x) + a_s.[i]
            y <- (y&&&p) + (y>>>89)
        let prim = y &&& (((bigint 1) <<< (l+1)) - 1I)
        let tmp_val = (prim &&& 1I)
        let h = prim >>> 1 |> int32
        let s = 1I - (tmp_val <<< 1) |> int64
        (h,s)