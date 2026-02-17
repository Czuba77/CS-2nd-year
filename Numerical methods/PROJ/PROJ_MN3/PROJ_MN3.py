# This is a sample Python script.
import numpy as np
import matplotlib.pyplot as plt
import pandas as pd
# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
def opening_csv(name, skip_first=False):
    if skip_first:
        data = np.genfromtxt(name, delimiter=',', skip_header=1)
    else:
        data = np.genfromtxt(name, delimiter=',')
    return data

def select_data_even(data,num_of_nodes):
    delta=(int)(data.shape[0]/num_of_nodes)
    return data[::delta]

def select_data_end_mainly(data, num_of_nodes):
    x = np.linspace(0, 1, num_of_nodes)
    x_transformed = np.cos(x * np.pi / 2)**2
    indices = (x_transformed * (data.shape[0] - 1)).astype(int)
    indices = np.unique(indices)
    return data[indices]

def phi(x,i,data):
    phix = 1
    for j in range(0,data.shape[0]):
        if i != j:
            phix *= (x-data[j][0])/(data[i][0]-data[j][0])
    return phix

def lagrange_interpolation(x,data):
    fx=0
    for i in range(0,data.shape[0]):
        fx+=data[i][1]*phi(x,i,data)
    return fx

def predict_val(mode,num_of_nodes,data):
    data,xmax,xmin=scale(data)
    data_s=select_data_even(data,num_of_nodes)
    data_p=np.zeros(data.shape[0])
    if mode == 'l':
        for i in range(0,data.shape[0]):
            data_p[i]=lagrange_interpolation(data[i][0],data_s)
    else:
        fun,coefs=cubic_spline_interpolation(data_s[:, 0],data_s[:, 1])
        for i in range(0, data.shape[0]):
            data_p[i] = fun(data[i][0])
    data=unscale(data,xmax,xmin)
    yArr =[data[:, 1],data_p]
    colorArr = ['red', 'blue']
    labelArr = ['Original', 'Predicted']
    make_plot_mul(data[:, 0],yArr,"Porownanie wartosci oryginalnej funkcji do interpolowanej","X","Y",colorArr,labelArr)

def make_plot_mul(x, yArr,title,xlabel,ylabel,colorArr,labelArr):
    plt.figure(figsize=(14, 8))
    for i in range(0, len(yArr)):
        plt.plot(x, yArr[i], color=colorArr[i],label=labelArr[i], linestyle='-', lw=1)
    plt.title(title)
    plt.xlabel(xlabel)
    plt.ylabel(ylabel)
    #plt.xlim(500,7300)
    #plt.ylim(6000,10000)
    plt.grid(True)
    plt.legend()
    plt.show()


def route_analysys(mode, data,yD,yU,dist_func):
    data, xmax, xmin = scale(data)
    node_counts = [5, 10, 25, 50]

    data_s = []
    data_p = []

    for count in node_counts:
        sample = dist_func(data, count)
        data_s.append(sample)
        data_p.append(np.zeros(data.shape[0]))

    for j in range(len(node_counts)):
        if mode == 'l':
            for i in range(data.shape[0]):
                data_p[j][i] = lagrange_interpolation(data[i][0], data_s[j])
        else:
            fun, _ = cubic_spline_interpolation(data_s[j][:, 0], data_s[j][:, 1])
            for i in range(data.shape[0]):
                data_p[j][i] = fun(data[i][0])

    data = unscale(data, xmax, xmin)


    fig, axs = plt.subplots(2, 2, figsize=(16, 10))
    axs = axs.flatten()

    for i in range(4):
        axs[i].plot(data[:, 0], data[:, 1], color='black', label='Original')
        axs[i].plot(data[:, 0], data_p[i], color='blue', label=f'Interpolated ({node_counts[i]} nodes)')
        axs[i].set_title(f'Interpolacja – {node_counts[i]} węzłów')
        axs[i].set_xlabel('X')
        axs[i].set_ylabel('Y')
        axs[i].set_ylim(yD,yU)
        if dist_func == select_data_end_mainly:
            data_s[i] = unscale(data_s[i], xmax, xmin)
        axs[i].scatter(data_s[i][:, 0], data_s[i][:, 1], color='red', label='Węzły', zorder=5)
        axs[i].grid(True)
        axs[i].legend()

    plt.tight_layout()
    plt.show()

def scale(data):
    xmax = max(data[:,0])
    xmin = min(data[:,0])
    for i in  range(0,data.shape[0]):
        data[i][0]=(data[i][0]-xmin)/(xmax-xmin)
    return data,xmax, xmin

def unscale(data,xmax,xmin):
    for i in  range(0,data.shape[0]):
        data[i][0]=data[i][0]*(xmax-xmin)+xmin
    return data

def unscale_one_dim(data,xmax,xmin):
    for i in  range(0,data.shape[0]):
        data[i]=data[i]*(xmax-xmin)+xmin
    return data

def cubic_spline_interpolation(x, y):
    n = len(x) - 1
    h = [x[i+1] - x[i] for i in range(n)]

    A = np.zeros((n+1, n+1))
    b = np.zeros(n+1)

    A[0][0] = 1
    A[n][n] = 1

    for i in range(1, n):
        A[i][i-1] = h[i-1]
        A[i][i] = 2*(h[i-1] + h[i])
        A[i][i+1] = h[i]
        b[i] = 3 * ((y[i+1] - y[i])/h[i] - (y[i] - y[i-1])/h[i-1])

    c = np.linalg.solve(A, b)

    a = y[:-1]
    b_coeff = []
    d = []

    for i in range(n):
        b_i = (y[i+1] - y[i])/h[i] - h[i]*(2*c[i] + c[i+1])/3
        d_i = (c[i+1] - c[i]) / (3*h[i])
        b_coeff.append(b_i)
        d.append(d_i)

    coeffs = []
    for i in range(n):
        coeffs.append((a[i], b_coeff[i], c[i], d[i]))

    def spline_function(x_val):
        for i in range(n):
            if x[i] <= x_val <= x[i+1]:
                dx = x_val - x[i]
                a_i, b_i, c_i, d_i = coeffs[i]
                return a_i + b_i*dx + c_i*dx**2 + d_i*dx**3
        return None
    return spline_function, coeffs

#track_1 = opening_csv('Hel_yeah.csv')
#track_1_sample=select_data_even(track_1,20)
#(1,track_1_sample,track_1)

track = opening_csv('MountEverest.csv',True)
route_analysys('s',track,6000, 9000,select_data_even)
track = opening_csv('MountEverest.csv',True)
route_analysys('l',track,6000, 9000,select_data_even)

track = opening_csv('Hel_yeah.csv',False)
route_analysys('s',track,0, 300,select_data_even)
track = opening_csv('Hel_yeah.csv',False)
route_analysys('l',track,0, 300,select_data_even)


track = opening_csv('SpacerniakGdansk.csv',True)
route_analysys('s',track,-2, 25,select_data_even)
track = opening_csv('SpacerniakGdansk.csv',True)
route_analysys('l',track,-2, 25,select_data_even)

track = opening_csv('SpacerniakGdansk.csv',True)
route_analysys('l',track,-2, 25,select_data_end_mainly)
track = opening_csv('SpacerniakGdansk.csv',True)
route_analysys('s',track,-2, 25,select_data_end_mainly)