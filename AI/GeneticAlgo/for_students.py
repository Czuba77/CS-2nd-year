from itertools import compress
import random
import time
import numpy as np
import matplotlib.pyplot as plt

from data import *

def initial_population(individual_size, population_size):
    return [[random.choice([True, False]) for _ in range(individual_size)] for _ in range(population_size)]

def fitness(items, knapsack_max_capacity, individual):
    total_weight = sum(compress(items['Weight'], individual))
    if total_weight > knapsack_max_capacity:
        return 0
    return sum(compress(items['Value'], individual))

def population_best(items, knapsack_max_capacity, population):
    best_individual = None
    population_fitness = []
    best_individual_fitness = -1
    for individual in population:
        individual_fitness = fitness(items, knapsack_max_capacity, individual)
        population_fitness.append(individual_fitness)
        if individual_fitness > best_individual_fitness:
            best_individual = individual
            best_individual_fitness = individual_fitness
    return best_individual, best_individual_fitness, population_fitness

def parents_choosing(population,population_fittness,parents_num):
    parents=[]
    fittnes_sum=sum(population_fittness)
    probability_arr=[]
    for i in range(0,len(population)):
        probability_arr.append(population_fittness[i]/fittnes_sum)
    parents_ind=np.random.choice(len(population),parents_num,True,probability_arr)
    for i in range(0,parents_num):
        parents.append(population[parents_ind[i]])
    return parents


def new_gen_creation(parents,parents_num):
    new_gen = []
    for i in range(0,int(parents_num/2)):
        for j in range(0,10):
            przegrodka = random.randint(1,len(parents[i])-1)
            child = parents[i][:przegrodka] + parents[i+int(parents_num/2)][przegrodka:]
            new_gen.append(child)
    return new_gen


def mutation(population):
    for i in range(0, len(population)):
        choice = random.randint(0,len(population[i])-1)
        population[i][choice]=not(population[i][choice])
    return population

def new_gen_update(new_gen,population,elite, fittnes_arr):
    fittnes_arr,population = zip(*sorted(zip(fittnes_arr,population)))
    population=list(population)
    fittnes_arr=list(fittnes_arr)
    elite_arr=population[(len(population)-elite):]
    for i in range(0,elite):
        choice = random.randint(0, len(population)-1)
        new_gen[choice]=elite_arr[i]
    return new_gen

items, knapsack_max_capacity = get_big()
print(items)

population_size = 100
generations = 200
n_selection = 20
n_elite = 20

start_time = time.time()
best_solution = None
best_fitness = 0
population_history = []
best_history = []
population = initial_population(len(items), population_size)
for _ in range(generations):
    population_history.append(population)
    # TODO: implement genetic algorithm
    #2. Wybór rodziców
    best_individual, best_individual_fitness, population_fittness = population_best(items, knapsack_max_capacity,population)
    parents=parents_choosing(population,population_fittness,n_selection)
    #3. Tworzenie kolejnego pokolenia
    new_gen=new_gen_creation(parents,n_selection)
    #4. Mutacja
    mutation(new_gen)
    #5. Aktualizacja populacji rozwiązań
    population=new_gen_update(new_gen,population,n_elite,population_fittness)
    if best_individual_fitness > best_fitness:
        best_solution = best_individual
        best_fitness = best_individual_fitness
    best_history.append(best_fitness)

end_time = time.time()
total_time = end_time - start_time
print('Best solution:', list(compress(items['Name'], best_solution)))
print('Best solution value:', best_fitness)
print('Time: ', total_time)

# plot generations
x = []
y = []
top_best = 10
for i, population in enumerate(population_history):
    plotted_individuals = min(len(population), top_best)
    x.extend([i] * plotted_individuals)
    population_fitnesses = [fitness(items, knapsack_max_capacity, individual) for individual in population]
    population_fitnesses.sort(reverse=True)
    y.extend(population_fitnesses[:plotted_individuals])
plt.scatter(x, y, marker='.')
plt.plot(best_history, 'r')
plt.xlabel('Generation')
plt.ylabel('Fitness')
plt.show()
