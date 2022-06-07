open HashFunctions
open StreamGenerator
open System.Diagnostics
open HashTableChain
open Microsoft.FSharp.Core.Operators
open SquareSumming
open CountSketch

// Elements in stream
let n : int = 10000000
// Unique elements in stream
let l : int = 6
let C_S = createStream n l 

// from www.random.org/bytes, generating 8 random bytes, setting last bytes bit to 1.
let random_a_uneven : uint64 = 5858726290542183271UL
// positive int less than 64
let lessthan64_l : int32 = 6
// a and b i [p] from random.org/bytes 89 bits randomly generated, then transformed to decimal value as written below
let random_a_p : bigint = 529974483324080114956256870I
let random_b_p : bigint = 27323842742462869155777889I

let mulshihash = new MulShift_Hashing(random_a_uneven, lessthan64_l) :> Generic_H_F
let mulmodprihash = new MulModPrime_Hashing(random_a_p, random_b_p, lessthan64_l) :> Generic_H_F

//TESTING MULTIPLY SHIFT RUNTIME 
let mulshi_time = 
    let time_count = new Stopwatch()
    let mutable mulshi_sum = 0UL
    time_count.Start()
    for i in C_S do
        let i_fst = (i.key)
        mulshi_sum <- mulshi_sum + uint64(mulshihash.hashed i_fst)
    time_count.Stop()
    time_count.Elapsed, mulshi_sum
printfn "Multiply shift hashing + summing time and sum result: %A" mulshi_time

//TESTING MULTIPLY MOD PRIME RUNTIME
let mulmod_time = 
    let time_count2 = new Stopwatch()
    let mutable mulmod_sum = 0UL
    time_count2.Start()
    for i in C_S do
        let i_fst = (i.key)
        mulmod_sum <- mulmod_sum + uint64(mulmodprihash.hashed i_fst)
    time_count2.Stop()
    time_count2.Elapsed, mulmod_sum
printfn "Multiply mod prime hashing + summing time and sum result: %A" mulmod_time

printfn "\nHASHTABLE TESTS"
let table_test = new HashTable(mulmodprihash)

printfn "TESTS FOR SET"
table_test.set(7UL, 32L)
let set_res = table_test.get(7UL)
printfn "Setting non existing - Expects: Some 32L, Got %A" set_res
table_test.set(7UL, 15L)
let set_res_2nd = table_test.get(7UL)
printfn "Setting already existing - Expects: Some 15L, Got %A" set_res_2nd

printfn "\nTESTS FOR GET"
let get_res_fail = table_test.get(12UL)
let get_res = table_test.get(7UL)
printfn "Trying to get non existing - Expects: None, Got %A" get_res_fail
printfn "Trying to get already existing - Expects: Some 15L, Got %A" get_res

printfn "\nTESTS FOR INCREMENT"
table_test.increment(7UL, 10L)
let incr_res = table_test.get(7UL)
table_test.increment(3UL, 15L)
let incr_set_res = table_test.get(3UL)
printfn "Incrementing already existing - Expects: Some 25L, Got %A" incr_res
printfn "Incrementing/setting non existing - Expects: Some 15L, Got %A" incr_set_res

printfn "SQUARE SUMMING TEST"
// Test on same n (number of elements) but with different l values.
let sqsum_n = 10000000
// From 1 to 15, used to be 20, but 20 resulted in program being killed
let sqsum_ls = [1; 5; 8; 10; 13; 15]
// Starting with MulShift hashing
printfn "Square summing with Multiply Shift Hashing"
for i in sqsum_ls do
    let tmp_stream = createStream sqsum_n i
    let tmp_hash = new MulShift_Hashing(random_a_uneven, lessthan64_l)
    let sq_summer = SquareSummer(tmp_hash)
    let stream_tolist = List.ofSeq tmp_stream
    let resu = stream_integration(stream_tolist, sq_summer)
    let time_count3 = Stopwatch()
    time_count3.Start()
    let prnt_res = resu.run()
    time_count3.Stop()
    printfn " l = %A\nTime: %A\nSquare Sum: %A" i time_count3.Elapsed prnt_res

printfn "Square summing with Multiply Mod Prime Hashing"
for i in sqsum_ls do
    let tmp_stream = createStream sqsum_n i
    let tmp_hash = new MulModPrime_Hashing(random_a_p, random_b_p, lessthan64_l)
    let sq_summer = SquareSummer(tmp_hash)
    let stream_tolist = List.ofSeq tmp_stream
    let resu = stream_integration(stream_tolist, sq_summer)
    let time_count4 = Stopwatch()
    time_count4.Start()
    let prnt_res = resu.run()
    time_count4.Stop()
    printfn " l = %A\nTime: %A\nSquare Sum: %A" i time_count4.Elapsed prnt_res

let rand_a_0 : bigint = 126885869669102854163944138I
let rand_a_1 : bigint = 392425927229767805008643723I
let rand_a_2 : bigint = 512737815644710089529680070I
let rand_a_3 : bigint = 584593371775862903194778891I
let a_arr = [rand_a_0;rand_a_1;rand_a_2;rand_a_3]
let l_s = [1;2;4;8]
let count_stream = createStream n 15
printfn "COUNT SKETCH"
for i in l_s do
    let tmp_hash1 = new MulModPrime_Hashing(random_a_p, random_b_p, i)
    let sq_summer1 = SquareSummer(tmp_hash1)
    let stream_tolist1 = List.ofSeq count_stream
    let resu1 = stream_integration(stream_tolist1, sq_summer1)
    let prnt_res1 = resu1.run()
    printfn "Exact square sum with l=%A: %A" i prnt_res1
for i in l_s do
    let four_uni = new four_uni_Hashing(i, a_arr)
    let count_sketch = CountSkether(four_uni)
    let stream_tolist = List.ofSeq count_stream
    let resu = stream_integration(stream_tolist, count_sketch)
    let time_count5 = Stopwatch()
    time_count5.Start()
    let prnt_res = resu.run()
    time_count5.Stop()
    printfn "Counsketch with l = %A\n Time: %A\nEstimate: %A" i time_count5.Elapsed prnt_res