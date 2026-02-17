function velocity_delta = velocity_difference(t)
% t - czas od startu rakiet
if t <= 0
    error('czas musi być większy od zera.');
end
u=2000;
m0=150000;
q=2700;
g=1.622
M=700;
vt=u*log(m0/(m0-q*t))-g*t;
velocity_delta = vt-M;

end