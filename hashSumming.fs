//file can be deleted...has been moved to program.fs
module HashSumming
open System.Diagnostics
open Microsoft.FSharp.Core.Operators

let createStream (n : int) (l : int) : seq<uint64 * int> =
    seq {
        // We generate a random uint64 number.
        let rnd = System.Random ()
        let mutable a = 0UL
        let b : byte [] = Array.zeroCreate 8
        rnd.NextBytes(b)
        let mutable x : uint64 = 0UL
        for i = 0 to 7 do
            a <- (a <<< 8) + uint64(b.[i])
        // We demand that our random number has 30 zeros on the least
        // significant bits and then a one.
        a <- (a ||| ((1UL <<< 31) - 1UL)) ^^^ ((1UL <<< 30)
            - 1UL)
        let mutable x = 0UL
        for i = 1 to (n/3) do
            x <- x + a
            yield (x &&& (((1UL <<< l) - 1UL) <<< 30), 1)
        for i = 1 to ((n + 1)/3) do
            x <- x + a
            yield (x &&& (((1UL <<< l) - 1UL) <<< 30), -1)
        for i = 1 to (n + 2)/3 do
            x <- x + a
            yield (x &&& (((1UL <<< l) - 1UL) <<< 30), 1)
    }

//Mul-shift hashing 
let MulShift_Hashing(x) = 
    let a : bigint = 5735218251591912617194I //random int 
    let l = 46 
    ((a * x)>>>(64-l))

let n : int = 1000000
let l : int = 10

let C_S = createStream n l 

let time_count = new Stopwatch() //stopwatch to monitor time

let mutable sum_of_shift = 0I //sum of mul shift 
time_count.Start()
for c in C_S do
    let c1 = bigint(fst c) //returns first tuple
    sum_of_shift <- sum_of_shift + MulShift_Hashing c1 
time_count.Stop()

printf "The Mul-Shift Sum = %A \n" sum_of_shift
printf "Time taken for Mul-Shift Sum : %A \n" time_count.Elapsed

//Mul-Mod Hash
//let MulModPrime_Hashing(x) = 
//     let a : bigint = 276355893122181903801078345I
//     let b : bigint = 257956312495689678942101343I
//     let l : int = 89
     let p : bigint = pown 2I l
     let eq : bigint = (a * x + b)
     let mutable eq2 = (eq &&& p) + (eq >>> l)
     if (eq2 > p) then eq2 <- eq2 - p  else eq2 <- eq2 
     eq2 % p 

printf "The Mul-Mod Sum = %A \n" sum_of_shift
printf "Time taken for Mul-Mod Sum : %A \n" time_count.Elapsed