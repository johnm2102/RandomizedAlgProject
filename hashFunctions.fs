module HashFunctions
open System.Numerics

// no bigints as stated in asg-text
type MulShift_Hashing(a:uint64,l:int32) =
    member this.l:int32 = l
    member this.hashed (x:uint64) = ((a*x) >>> (64-l) |> int32)

// l is less than 64 so int32 should be more than efficient
// I is literal for bigint
// 1<<<89 is same as 2^89, since 1>>1 = 2 and is the same as 2^1 = 2
type MulModPrime_Hashing(a:bigint,b:bigint,l:int32) = 
    let p:bigint = (1I<<<89) - 1I
    member this.l:int32 = l
    member this.hashed (x:int32) = a*(bigint x)+b |>
        fun x -> (x&&&p) + (x>>>89) |> //from Assignment 3 we know that this is equivalent to x mod p
        fun x -> x&&&(1I<<<l)-1I |> // doing mod 2^l
        fun x -> if x < p then x else x-p // subtracting p if x > p
        |> int32 //piping all the above into an int32 type