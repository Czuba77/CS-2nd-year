import copy
import math
import numpy as np
import matplotlib.pyplot as plt
import time as timer
#f 3 cyfra ind
def create_b_vec(N,f = 1):
    b = []
    for n in range(0,N):
        b.append(math.sin(n * (f + 1)))
    b=np.array(b)
    return b

#e 4 cyfra ind
def create_A_matrix(N, a1 = 5 + 0,a2 = -1,a3 = -1):
    A_matrix = np.zeros((N, N))
    np.fill_diagonal(A_matrix,a1)
    np.fill_diagonal(A_matrix[:, 1:], a2)
    np.fill_diagonal(A_matrix[1:, :], a2)
    np.fill_diagonal(A_matrix[:, 2:], a3)
    np.fill_diagonal(A_matrix[2:, :], a3)
    return A_matrix

def make_plot(x, y,title,xlabel,ylabel,color):
    plt.figure(figsize=(14, 8))
    plt.plot(x, y, color=color, linestyle='-', lw=1)
    plt.title(title)
    plt.xlabel(xlabel)
    plt.ylabel(ylabel)
    plt.grid(True)
    plt.yscale('log')
    plt.show()

def make_plot_mul(x, yArr,title,xlabel,ylabel,colorArr,labelArr,isLinear=False):
    plt.figure(figsize=(14, 8))
    for i in range(0, len(yArr)):
        plt.plot(x, yArr[i], color=colorArr[i],label=labelArr[i], linestyle='-', lw=1)
    plt.title(title)
    plt.xlabel(xlabel)
    plt.ylabel(ylabel)
    plt.grid(True)
    plt.legend()
    if not isLinear:
        plt.yscale('log')
    plt.show()

def Gauss_Seidl(A_matrix,b,N,cutoff=1e-9):
    t = timer.time()
    iter_count=0
    D=np.diag(np.diag(A_matrix))
    U = np.triu(A_matrix, 1)
    L = np.tril(A_matrix, -1)
    x = np.ones(N)
    M = -1 * np.linalg.solve(D + L,U)
    w= np.linalg.solve(D + L,b)
    norm_history = []
    residue_norm = np.linalg.norm(A_matrix @ x - b)
    norm_history.append(residue_norm)

    while residue_norm > cutoff and iter_count < 1000:
        x=M @ x + w
        residue_norm = np.linalg.norm(A_matrix @ x - b)
        iter_count+=1
        norm_history.append(residue_norm)
    t =  timer.time() - t
    return x,norm_history,iter_count,t

def Jacobi(A_matrix,b,N,cutoff=1e-9):
    t = timer.time()
    iter_count = 0
    U = np.triu(A_matrix, 1)
    L = np.tril(A_matrix, -1)
    D = np.diag(np.diag(A_matrix))
    M = np.linalg.solve(-D,L+U)
    w = np.linalg.solve(D,b)

    x = np.ones(N)
    norm_history = []
    residue_norm = np.linalg.norm(A_matrix @ x - b)
    norm_history.append(residue_norm)
    while residue_norm > 1e-12 and iter_count < 1000:
        x = M @ x + w
        residue_norm = np.linalg.norm(A_matrix @ x - b)
        iter_count+=1
        norm_history.append(residue_norm)
    t = timer.time() - t
    return x, norm_history, iter_count, t

def lu_solve(A_matrix,N):
    U = copy.copy(A_matrix)
    L = np.eye(N)
    for i in range(2,N+1):
        for j in range(1,i):
            L[i-1,j-1] = U[i-1,j-1]/U[j-1,j-1]
            U[i-1,:] = U[i-1,:] - L[i-1,j-1]*U[j-1,:]
    return L,U

def LU(A_matrix,b,N,cutoff=1e-9):
    t = timer.time()
    L,U=lu_solve(A_matrix,N)
    y = np.linalg.solve(L, b)
    x = np.linalg.solve(U, y)
    residue_norm = np.linalg.norm(A_matrix @ x - b)
    t = timer.time() - t
    return x,residue_norm,t

def zad_b(N):
    A_matrx = create_A_matrix(N)
    b = create_b_vec(N)
    x, norm_history, iter_count, time = Gauss_Seidl(A_matrx, b,N)
    print(f"Gauss-Seidl: Ilosc Iteracji: {iter_count}, czas: {time}")
    make_plot(range(len(norm_history)), norm_history, "Zmiana residuum dla kolejnych iteracji w metodzie Gaussa-Seidla",
              "Iteracje", "Residuum", 'green')

    A_matrx = create_A_matrix(N)
    b = create_b_vec(N)

    x, norm_history, iter_count, time = Jacobi(A_matrx, b,N)
    print(f"Jacobi: Ilosc Iteracji: {iter_count}, czas: {time}")
    make_plot(range(len(norm_history)), norm_history, "Zmiana residuum dla kolejnych iteracji w metodzie Jacobiego",
              "Iteracje", "Residuum", 'green')

def zad_c(N):
    A_matrx = create_A_matrix(N, 3, -1, -1)
    b = create_b_vec(N)

    x, norm_history, iter_count, time = Gauss_Seidl(A_matrx, b,N)
    print(f"Gauss-Seidl: Ilosc Iteracji: {iter_count}, czas: {time}")
    make_plot(range(len(norm_history)), norm_history,
              "Zmiana residuum dla kolejnych iteracji w metodzie Gaussa-Seidla dla a1=3", "Iteracje", "Residuum",
              'green')

    A_matrx = create_A_matrix(N, 3, -1, -1)
    b = create_b_vec(N)

    x, norm_history, iter_count, time = Jacobi(A_matrx, b,N)
    print(f"Jacobi: Ilosc Iteracji: {iter_count}, czas: {time}")
    make_plot(range(len(norm_history)), norm_history,
              "Zmiana residuum dla kolejnych iteracji w metodzie Jacobiego dla a1=3", "Iteracje", "Residuum", 'green')


def zad_d(N):
    A_matrx = create_A_matrix(N)
    b = create_b_vec(N)

    x, residue_norm,time = LU(A_matrx, b,N)
    print(f"LU: Norma residuum: {residue_norm}, czas: {time}")

def zad_e():
    N=[100, 500, 1000, 2000, 3000,4000,5000]
    G_time=[]
    J_time=[]
    L_time = []
    for n in N:
        A_matrx = create_A_matrix(n)
        b = create_b_vec(n)
        x, norm_history, iter_count, time = Gauss_Seidl(A_matrx, b,n)
        G_time.append(time)
        A_matrx = create_A_matrix(n)
        b = create_b_vec(n)
        x, norm_history, iter_count, time = Jacobi(A_matrx, b,n)
        J_time.append(time)
        A_matrx = create_A_matrix(n)
        b = create_b_vec(n)
        x, residue_norm,time = LU(A_matrx, b,n)
        L_time.append(time)
    colorArr = ['green', 'red', 'blue']
    labelArr = ['Gauss-Seidel', 'Jacobi', 'LU']
    yArr = [G_time, J_time, L_time]
    make_plot_mul(N, yArr, "Wykresy czasu wykoanania w zaleźności od wielkości macierzy", "Wielkość N", "Czas",
                  colorArr, labelArr)
    make_plot_mul(N, yArr, "Wykresy czasu wykoanania w zaleźności od wielkości macierzy", "Wielkość N", "Czas",
                  colorArr, labelArr, True)


index = 201096
c=9
d=6
N=1200 + 10*c + d

#zad_b(N)

#zad_c(N)

zad_d(N)

zad_e()