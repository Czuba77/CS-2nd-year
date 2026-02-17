function [rand_counts, counts_mean,circles, a, b, r_max] = generate_circles(n_max)
    a = randi([150, 250]);
    b = randi([50, 100]);
    
    circles = zeros([n_max,3]);
    rand_counts = zeros([1,n_max]);
    r_max = randi([20, 50]);
    
    rand_count_pom=0;
    czyPoprawny = false;
    for i = 1:n_max
        while czyPoprawny == false
            r = r_max *rand();
            X = r + (a - 2*r) * rand();
            Y = r + (b - 2*r) * rand();
            rand_count_pom=rand_count_pom+3;
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
        rand_counts(i)=rand_count_pom;
        rand_count_pom=0;
    end
    

    counts_mean=cumsum(rand_counts)./(1:width(rand_counts));

    
    hold on
    subplot(2,1,1);
    plot(rand_counts);
    xlabel("Ilosc kol");
    ylabel("Ilosc losowan");
    title("Ilosc losowan")

    subplot(2,1,2);
    plot(counts_mean);
    xlabel("Ilosc kol");
    ylabel("Wartosc sredniej");
    title("Srednia losowan ")
    hold off
end