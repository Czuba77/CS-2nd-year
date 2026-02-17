# This is a sample Python script.
import numpy as np
# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.

def zad1():
    M=100
    przedzialy=[]
    for i in range(0,10):
        przedzialy.append(0)
    for i in range(0,100000):
        rand_numb=np.random.random()
        rand_numb=100*rand_numb+50
        for j in range(0,10):
            if rand_numb < M / 10 * (j + 1) + 50:
                przedzialy[j] = przedzialy[j] + 1
                break
    print(przedzialy)

def zad2():
    przedzialy=[]
    for i in range(0,4):
        przedzialy.append(0)
    for i in range(0,100000):
        rand_numb=np.random.random()
        if rand_numb < 0.3:
            przedzialy[0]+=1
        elif rand_numb < 0.4:
            przedzialy[1] += 1
        elif rand_numb < 0.8:
            przedzialy[2] += 1
        else:
            przedzialy[3] += 1
    print( przedzialy)


def zad3():
    M = 100
    fmax=150
    epsilon=fmax/1000
    U1=1.0
    U2=1.0
    losowania=0
    przedzialy = []
    num_of_num=100000
    for i in range(0,10):
        przedzialy.append(0)
    for i in range(0,num_of_num):

        rand_numb = np.random.random() + (1 - i / num_of_num)
        rand_numb = 100 * rand_numb + 50
        losowania+=1
        while rand_numb>150.0:
            rand_numb=np.random.random()+(i/num_of_num)
            rand_numb=100*rand_numb+50
            losowania+=1

        for j in range(0,10):
            if rand_numb < M / 10 * (j + 1) + 50:
                przedzialy[j] = przedzialy[j] + 1
                break
    print(przedzialy)
    print(losowania)

# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    zad1()
    zad2()
    zad3()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
