import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from pandas import Timestamp


def opening_csv():
    data=pd.read_csv('wig20_w.csv')
    data=data[['Data','Zamkniecie']]
    data['Data'] = pd.to_datetime(data['Data'])
    data=data[data['Data']>pd.to_datetime("2005-12-31")]
    data = data.reset_index(drop=True)
    data.index += 0
    return data

#def my_ema_rec(data, N, index, depth):
#    if depth == N or index-depth == 0:
#        return data['Zamkniecie'][index-depth]
#    else:
#        alfa=2/(N+1)
#        return alfa*data['Zamkniecie'][index-depth]+(1-alfa)*my_ema_rec(data,N,index,depth+1)
#def my_ema(data,N):
#    emaArr=[]
#    emaArr.append(data['Zamkniecie'][0])
#    for i in range(1,len(data)):
#        ema_value=my_ema_rec(data,N,i,0)
#        emaArr.append(ema_value)
#    return emaArr

def my_ema(arr,N):
    emaArr=[]
    alfa = 2 / (N + 1)
    emaArr.append(arr[0])
    for i in range(1,len(arr)):
        ema_value=alfa*arr[i]+(1-alfa)*emaArr[i-1]
        emaArr.append(ema_value)
    return emaArr

def my_macd(data):
    macd=[]
    ema12 = my_ema(data['Zamkniecie'], 12)
    ema26 = my_ema(data['Zamkniecie'], 26)
    for i in range(0,len(data)):
        macd.append(ema12[i]-ema26[i])
    return macd

def my_signal(macd):
    signal=my_ema(macd,9)
    return signal

def find_intersections(macd, signal, Date):
    sell=[[],[]]
    buy=[[],[]]
    for i in range(1, len(macd)):
        if macd[i]<signal[i] and macd[i - 1]>signal[i - 1]:
            buy[0].append(macd[i])
            buy[1].append(Date[i])
        elif macd[i]>signal[i] and macd[i - 1]<signal[i - 1]:
            sell[0].append(macd[i])
            sell[1].append(Date[i])
    return sell,buy

def find_intersections_prices(macd, signal, Date, cap):
    sell=[[],[]]
    buy=[[],[]]
    for i in range(1, len(macd)):
        if macd[i]<signal[i] and macd[i - 1]>signal[i - 1]:
            buy[0].append(cap[i])
            buy[1].append(Date[i])
        elif macd[i]>signal[i] and macd[i - 1]<signal[i - 1]:
            sell[0].append(cap[i])
            sell[1].append(Date[i])
    return sell,buy

def make_plot(title,x,y,xlabel,ylabel,color):
    plt.figure(figsize=(14, 8))
    plt.plot(x, y, color=color, linestyle='-', lw=1)
    plt.title(title)
    plt.xlabel(xlabel)
    plt.ylabel(ylabel)
    plt.grid(True)
    plt.show()

def make_plot_section(title,x,y,xlabel,ylabel,lineLabel,color,xlim,ylim, SellSignal, BuySignal):
    plt.figure(figsize=(14, 8))
    plt.plot(x, y, color=color,label=lineLabel ,linestyle='-', lw=1)
    cond = False
    for sell_time in SellSignal[1]:
        plt.axvline(sell_time, color='magenta',label="Sell" if not cond else "",  linestyle='--', linewidth=1)
        cond=True
    cond = False
    for buy_time in BuySignal[1]:
        plt.axvline(buy_time, color='green',label="Buy" if not cond else "", linestyle='--', linewidth=1)
        cond = True
    plt.title(title)
    plt.xlabel(xlabel)
    plt.ylabel(ylabel)
    plt.xlim(xlim)
    if ylim!=0:
        plt.ylim(ylim)
    plt.grid(True)
    plt.legend()
    plt.show()

def make_multiple_plot(title,yArr,x,xlabel,ylabel,colorArr,labelArr):
    plt.figure(figsize=(14, 8))
    for i in range(0,len(yArr)):
        plt.plot(x,yArr[i],  color=colorArr[i],label=labelArr[i], linestyle='-', lw=1)
    plt.title(title)
    plt.xlabel(xlabel)
    plt.ylabel(ylabel)
    plt.grid(True)
    plt.show()

def makeSellBuyPlot(title,yArr,x,xlabel,ylabel,colorArr,labelArr,SellSignal,BuySignal):
    plt.figure(figsize=(14, 8))
    for i in range(0,len(yArr)):
        plt.plot(x,yArr[i],  color=colorArr[i],label=labelArr[i], linestyle='-', lw=1)
    plt.scatter(BuySignal[1], BuySignal[0], color='green', label='Kupno', marker='o',zorder=3,s=20)
    plt.scatter(SellSignal[1],SellSignal[0], color='magenta', label='Sprzedaż', marker='o',zorder=3,s=20)
    plt.title(title)
    plt.xlabel(xlabel)
    plt.ylabel(ylabel)
    plt.legend()
    plt.grid(True)
    plt.show()

def makeSellBuyPlotSection(title, yArr, x, xlabel, ylabel, colorArr,labelArr, SellSignal, BuySignal,xlim):
    plt.figure(figsize=(14, 8))
    for i in range(0, len(yArr)):
        plt.plot(x, yArr[i], color=colorArr[i],label=labelArr[i], linestyle='-', lw=1)
    cond = False
    for buy_time in BuySignal[1]:
        plt.axvline(buy_time, color='green',label="Kupno" if not cond else "", linestyle='--', linewidth=1)
        cond = True
    cond = False
    for sell_time in SellSignal[1]:
        plt.axvline(sell_time, color='magenta',label="Sprzedaż" if not cond else "",  linestyle='--', linewidth=1)
        cond=True
    plt.xlim(xlim)
    plt.title(title)
    plt.xlabel(xlabel)
    plt.ylabel(ylabel)
    plt.grid(True)
    plt.legend()
    plt.show()


data=opening_csv()
#print(data)
macd=my_macd(data)
signal=my_signal(macd)
SellSignal,BuySignal = find_intersections_prices(macd,signal,data['Data'], data['Zamkniecie'])
#Wykres gieldy
#make_plot("Wykres cen akcji giełdy",data['Data'], data['Zamkniecie'],'Data','Ceny akcji','green')
plotArr=[data['Zamkniecie']]
colorArr=['green']
labelArr=['Wartosc akcji']
#Wykres gieldy
makeSellBuyPlot("Wykres cen akcji giełdy",plotArr,data['Data'],'Data','Ceny akcji',colorArr,labelArr,SellSignal,BuySignal)
SellSignal,BuySignal = find_intersections(macd,signal,data['Data'])
plotArr=[macd,signal]
colorArr=['blue','red']
labelArr=['MACD','SIGNAL']
#Wykresy sekcji macd signal i puntkow przeciecia
#makeSellBuyPlot("Wykres MACD SIGNAL oraz punktów przecięcia",plotArr,data['Data'],'Data','Wartosc',colorArr,labelArr,SellSignal,BuySignal)

#Przyblizone macd signal i punkty przeciecia
#make_plot_section("Wykres cen akcji giełdy",data['Data'], data['Zamkniecie'],'Data','Zamkniecie',"Ceny akcji",'green',(Timestamp('2007-10-15'),Timestamp('2009-02-15')),0, SellSignal, BuySignal)
#makeSellBuyPlotSection("Wykres MACD SIGNAL oraz punktów przecięcia",plotArr,data['Data'],'Data','Wartosc',colorArr,labelArr,SellSignal,BuySignal,(Timestamp('2007-10-15'),Timestamp('2009-02-15')))
#make_plot_section("Wykres cen akcji giełdy",data['Data'], data['Zamkniecie'],'Data','Zamkniecie',"Ceny akcji",'green',(Timestamp('2009-04-15'),Timestamp('2011-04-15')),(1500,3000), SellSignal, BuySignal)
#makeSellBuyPlotSection("Wykres MACD SIGNAL oraz punktów przecięcia",plotArr,data['Data'],'Data','Wartosc',colorArr,labelArr,SellSignal,BuySignal,(Timestamp('2009-04-15'),Timestamp('2011-04-15')))
#make_plot_section("Wykres cen akcji giełdy",data['Data'], data['Zamkniecie'],'Data','Zamkniecie',"Ceny akcji",'green',(Timestamp('2013-06-15'),Timestamp('2014-03-15')),(2000,3000), SellSignal, BuySignal)
#makeSellBuyPlotSection("Wykres MACD SIGNAL oraz punktów przecięcia",plotArr,data['Data'],'Data','Wartosc',colorArr,labelArr,SellSignal,BuySignal,(Timestamp('2013-06-15'),Timestamp('2014-03-15')))


#make_plot("MACD",data['Data'],macd,'Data','Wartosc MACD','blue')
#make_plot("SIGNAL",data['Data'],signal,'Data','Wartosc MACD','red')
#make_multiple_plot("Wykres gieldu",plotArr,data['Data'],'Data','Wartosc',colorArr,labelArr)



Value=0.0
Capital=1000.0
NumberOfShares=0.0
HistoryOftransactions=[]
ValueTimeline=[]
DateWithoutWrongMacd=data['Data'][26:]
for i in range(26,len(data)):
    if np.isin(data['Data'][i],BuySignal) and Capital != 0.0:
        HistoryOftransactions.append(Capital)
        a=data['Zamkniecie'][i]
        NumberOfShares = Capital/float(a)
        print(f"{data['Data'][i]} Kupno: {float(NumberOfShares)} za {float(Capital):,.2f} PLN")
        Capital=0.0
    elif np.isin(data['Data'][i],SellSignal) and NumberOfShares != 0.0:
        Capital=float(NumberOfShares)*data['Zamkniecie'][i]
        HistoryOftransactions.append(Capital)
        print(f"{data['Data'][i]} Sprzedaz: {float(NumberOfShares)} za {float(Capital):,.2f} PLN")
        NumberOfShares=0.0
    ValueTimeline.append(Capital+NumberOfShares*data['Zamkniecie'][i])

NumberOfBeneficialTrans=0
NumberOfUnfavorableTrans=0
mean_ben=0.0
mean_un = 0.0
r=int(len(HistoryOftransactions)/2)
for i in range(0,r):
    if HistoryOftransactions[2*i] > HistoryOftransactions[2*i+1]:
        mean_un += HistoryOftransactions[2*i+1]/HistoryOftransactions[2*i] * 100 -100
        NumberOfUnfavorableTrans=NumberOfUnfavorableTrans+1
    else:
        mean_ben += HistoryOftransactions[2*i+1]/HistoryOftransactions[2*i] * 100 -100
        NumberOfBeneficialTrans=NumberOfBeneficialTrans+1
mean_un=mean_un/NumberOfUnfavorableTrans
mean_ben=mean_ben/NumberOfBeneficialTrans
print(f"Liczba transakcji: {int(len(HistoryOftransactions)/2)}")
print(f"Liczba opłacalnych transakcji: {NumberOfBeneficialTrans}")
print(f"Liczba nieopłacalnych transakcji: {NumberOfUnfavorableTrans}")
print(f"Sredni zysk: {mean_ben}%")
print(f"Srednia strata: {mean_un}%")
#make_plot("Kapitalizacja Portfela",DateWithoutWrongMacd,ValueTimeline,'Data','Kapitalizacja','red')