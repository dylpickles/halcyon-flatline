import math

def pFactor(k: int):
    factorization = []

    Compost = True #checks if composite
    Prime = False
    new = k
    currTest = 2

    while (Compost):
        while (True):
            if (new%currTest == 0):
                factorization.append(currTest)
                new = int(new/currTest)
                currTest = 2
            elif (currTest == math.ceil(math.sqrt(new))):
                if len(factorization) == 0:
                    Prime = True
                else:
                    Prime = False
                    factorization.append(new)
                Compost = False
                break
            else:
                currTest = currTest+1

    if Prime:
        print(str(k) + " is Prime.")
    else:
        print("The prime factorization of "+str(k)+" is ", end ="")
        counter = len(factorization)
        for num in factorization:
            if counter > 1:
                print(str(num)+"*", end ="")
                counter = counter-1
            else:
                print(str(num), end ="")
        print(".")
