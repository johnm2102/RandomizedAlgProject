open HashFunctions
open StreamGenerator
open System.Diagnostics
open Microsoft.FSharp.Core.Operators

// let timer to_time = 
//     let timer = Stopwatch()
//     timer.Start()
//     let func_res = to_time()
//     let time_res = timer.ElapsedMilliseconds
//     timer.Stop()
//     func_res, time_res

let n : int = 100000000
let l : int = 100
let C_S = createStream n l 

// from www.random.org/bytes, generating 8 random bytes, setting last bytes bit to 1.
let random_a_uneven : uint64 = 5858726290542183271UL
// positive int less than 64
let lessthan64_l : int32 = 41
// a and b i [p] from random.org/bytes 89 bits randomly generated, then transformed to decimal value as written below
let random_a_p : bigint = 529974483324080114956256870I
let random_b_p : bigint = 27323842742462869155777889I

let mulshihash = new MulShift_Hashing(random_a_uneven, lessthan64_l)
let mulmodprihash = new MulModPrime_Hashing(random_a_p, random_b_p, lessthan64_l)

//TESTING MULTIPLY SHIFT RUNTIME 
let mulshi_time = 
    let time_count = new Stopwatch()
    let mutable mulshi_sum = 0UL
    time_count.Start()
    for i in C_S do
        let i_fst = (fst i)
        mulshi_sum <- mulshi_sum + uint64(mulshihash.hashed i_fst)
    time_count.Stop()
    time_count.Elapsed, mulshi_sum
printfn "Multiply shift hashing + summing time and sum result: %A" mulshi_time

//TESTING MULTIPLY MOD PRIME RUNTIME


//MUL-MOD-PRIME RUNTIME
// let time_count3 = new Stopwatch()
// time_count3.Start()
// let MulModPrime_Hashing(x) = 
//      let a : bigint = 276355893122181903801078345I
//      let b : bigint = 257956312495689678942101343I
//      let l : int = 58
//      let q : int = 89
//      let p : bigint = pown 2I q
//      let eq : bigint = (a * x + b)
//      let mutable eq2 = (eq &&& p) + (eq >>> q)
//      if (eq2 > p) then eq2 <- eq2 - p  else eq2 <- eq2 
//      eq2 % pown 2I l 
// time_count3.Stop()
// printf "Mul-Mod Prime Time: %A \n" time_count3.Elapsed

// //MUL-MOD-PRIME SUM RUNTIME 
// let time_count4 = new Stopwatch()
// let mutable sum_of_modprime = 0I //sum of mul shift 
// time_count4.Start()
// for c in C_S do
//     let c2 = bigint(fst c) //returns first tuple
//     sum_of_modprime <- sum_of_modprime + MulModPrime_Hashing c2 
// time_count4.Stop()
// printf "Mul-Mod Prime Sum = %A \n" sum_of_modprime
// printf "Mul-Mod Prime Sum Time: %A \n" time_count4.Elapsed


