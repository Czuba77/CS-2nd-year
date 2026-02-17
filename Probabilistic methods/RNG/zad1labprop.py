M=2**31
a=69069
n=1000000
c=1
X=15
arr=[]
for i in range(0,10):
    arr.append(0)

for i in range(0,n):
    X=(a*X+c)%M
    for j in range(0,10):
        if X < M/10*(j+1):
            arr[j]=arr[j]+1
            break

print(arr)