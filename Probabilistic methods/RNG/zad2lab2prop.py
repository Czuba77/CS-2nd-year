M=2**31
p=7
q=3
b=[1,1,0,1,0,0,1]
#b=0b1101001
n=1000000
number=0
arr=[]
for i in range(0,10):
    arr.append(0)

for i in range(0,n):
    for j in range(p,30):
        b.append(b[j-p]^b[j-q])
    number = 0
    for j in range(1,31):
        if b[j-1]==1:
            number = number + 2**j
    for j in range(0,10):
        if number < M/10*(j+1):
            arr[j]=arr[j]+1
            break
    b=b[23:]
print(arr)