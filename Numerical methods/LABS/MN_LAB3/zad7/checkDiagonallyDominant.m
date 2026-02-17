function isDiagDom = checkDiagonallyDominant(A)
[n, ~] = size(A);
isDiagDom = true;
for i = 1:n
    if abs(A(i,i)) < sum(abs(A(i,:))) - abs(A(i,i))
        isDiagDom = false;
        return;
    end
end
end