# This is a sample Python script.
import random
# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.



def przydziel(przedzialy,indexy,rand_numb):
    suma_przedzialow=0.0
    for i in range(0,len(przedzialy)):
        suma_przedzialow+=przedzialy[i]
        if rand_numb < suma_przedzialow:
            return indexy[i]


def zad1():
    tabela=[[0,0,0,0],[0,0,0,0],[0,0,0,0],[0,0,0,0]]
    p_x=[0.4,0.1,0.1,0.4]
    i_x=[0,1,2,3]
    p_y0=[0.75,0.25]
    i_y0=[1,3]
    p_y1=[1.0]
    i_y1=[1]
    p_y2=[0.5,0.5]
    i_y2=[2,3]
    p_y3=[0.625,0.375]
    i_y3=[1,2]
    for i in range(0,100000):
        rand_num = random.random()
        ind_x=przydziel(p_x,i_x,rand_num)
        if ind_x == 0:
            rand_num = random.random()
            ind_y=przydziel(p_y0,i_y0,rand_num)
        elif ind_x == 1:
            rand_num = random.random()
            ind_y = przydziel(p_y1, i_y1, rand_num)
        elif ind_x == 2:
            rand_num = random.random()
            ind_y = przydziel(p_y2, i_y2, rand_num)
        else:
            rand_num = random.random()
            ind_y = przydziel(p_y3, i_y3, rand_num)
        tabela[ind_x][ind_y]+=1
    print(tabela)



# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    zad1()


# See PyCharm help at https://www.jetbrains.com/help/pycharm/