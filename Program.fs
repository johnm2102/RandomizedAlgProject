//open HashFunctions
//open StreamGenerator
open System.Diagnostics
open Microsoft.FSharp.Core.Operators

//1c. Testing the runtime of the hashfunctions and sum of the hash functions
//creating a stream. Tried to do it using open StreamGen but error kept popping up, so i had a try without it
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

let n : int = 1000000
let l : int = 10
let C_S = createStream n l 

//TESTING MULTIPLY SHIFT RUNTIME 
let time_count = new Stopwatch()
time_count.Start()
let MulShift_Hashing(x) = 
    let a : bigint = 5735218251591912617194I //random int 
    let l = 46 
    ((a * x)>>>(64-l))
time_count.Stop()
printf "Multiply Shift Time: %A \n" time_count.Elapsed

//TESTING MULTIPLY SHIFT SUM RUNTIME 
let time_count2 = new Stopwatch()
let mutable sum_of_shift = 0I //sum of mul shift 
time_count2.Start()
for c in C_S do
    let c1 = bigint(fst c) //returns first tuple
    sum_of_shift <- sum_of_shift + MulShift_Hashing c1 
time_count2.Stop()
printf "Multiply Shift Sum = %A \n" sum_of_shift
printf "Multiply Shift Sum Time: %A \n \n" time_count2.Elapsed

//MUL-MOD-PRIME RUNTIME
let time_count3 = new Stopwatch()
time_count3.Start()
let MulModPrime_Hashing(x) = 
     let a : bigint = 276355893122181903801078345I
     let b : bigint = 257956312495689678942101343I
     let l : int = 89
     let p : bigint = pown 2I l
     let eq : bigint = (a * x + b)
     let mutable eq2 = (eq &&& p) + (eq >>> l)
     if (eq2 > p) then eq2 <- eq2 - p  else eq2 <- eq2 
     eq2 % p
time_count3.Stop()
printf "Mul-Mod Prime Time: %A \n" time_count3.Elapsed

//MUL-MOD-PRIME SUM RUNTIME 
let time_count4 = new Stopwatch()
let mutable sum_of_modprime = 0I //sum of mul shift 
time_count4.Start()
for c in C_S do
    let c2 = bigint(fst c) //returns first tuple
    sum_of_modprime <- sum_of_modprime + MulModPrime_Hashing c2 
time_count4.Stop()
printf "Mul-Mod Prime Sum = %A \n" sum_of_shift
printf "Mul-Mod Prime Sum Time: %A \n" time_count4.Elapsed


