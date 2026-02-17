function [circles, a, b, r_max] = generate_circles(n_max)
    a = randi([150, 250]);
    b = randi([50, 100]);
    
    circles = zeros([n_max,3]);
    
    r_max = randi([20, 50]);
    
    czyPoprawny = false;
    for i = 1:n_max
        while czyPoprawny == false
            r = r_max *rand();
            X = r + (a - 2*r) * rand();
            Y = r + (b - 2*r) * rand();

            czyPoprawny = true;
            for j=1:i-1
                odl = sqrt((X-circles(j,1))^2+(Y-circles(j,2))^2) ;
                if odl <= r + circles(j,3) 
                    czyPoprawny = false;
                end
            end
        end
        circles(i,:)=[X,Y,r];
        czyPoprawny = false;
    end

end
 