import numpy as np
import random as ran
import math as math
def initialize_centroids_forgy(data, k):
    # TODO implement random initialization
    indices = np.random.choice(data.shape[0], k, replace=False)
    return data[indices]
'''
def initialize_centroids_kmeans_pp(data, k):
    indicies=[]
    indicies.append(ran.randint(0,data.shape[0]))
    for i in range(1,k):
        indicies.sort()
        distances=[]
        distances.append(indicies[0])#odl pierwszy poczat-punkt
        distances.append(data.shape[0]-indicies[-1])#odl ostatni punkt-koniec
        for j in range(0,len(indicies)-1):
            distances.append(indicies[j+1]-indicies[j])
        biggest_dis=-1
        bd_ind=0
        for j in range(0,len(distances)):
            if(biggest_dis<distances[j]):
                biggest_dis=distances[j]
                bd_ind=j
        if bd_ind==0 and 0 not in indicies:
            indicies.append(0)
        elif bd_ind==1 and data.shape[0] not in indicies:
            indicies.append(data.shape[0]-1)
        else:
            indicies.append(indicies[bd_ind-2]+biggest_dis//2)
    indicies.sort()
    selected_data=[]
    for i in indicies:
        selected_data.append(data[i])
    return  np.array(selected_data)
'''

def initialize_centroids_kmeans_pp(data, k):
    centroids = np.zeros((k, data.shape[1]))
    centroids[0] = data[ran.randint(0,data.shape[0]-1)]
    for i in range(1, k):
        distances = np.zeros(data.shape[0])
        for j in range(data.shape[0]):
            distances[j]=float('inf')
            for cen in centroids[:i]:
                distmp=np.sqrt(np.sum((data[j] - cen) ** 2))
                if distmp<distances[j]:
                    distances[j] = distmp
        centroids[i] = data[distances.argmax()]
    return centroids


def assign_to_cluster(data, centroid):
    # TODO find the closest cluster for each data point
    assign_vec = []
    for i in range(0,len(data)):
        smallest_dis=float('inf')
        sd_ind=0
        for j in range(0,len(centroid)):
            dis=np.sqrt(np.sum((data[i] - centroid[j]) ** 2))
            if dis < smallest_dis:
                smallest_dis=dis
                sd_ind=j
        assign_vec.append(sd_ind)
    return np.array(assign_vec)

def update_centroids(data, assignments):
    # TODO find new centroids based on the assignments
    assignments = np.array(assignments)
    centroids = []
    for i in np.unique(assignments):
        points_in_cluster = data[assignments == i]
        centroid = np.mean(points_in_cluster, axis=0)
        centroids.append(centroid)
    return np.array(centroids)


def mean_intra_distance(data, assignments, centroids):
    return np.sqrt(np.sum((data - centroids[assignments, :])**2))

def k_means(data, num_centroids, kmeansplusplus= False):
    # centroids initizalization
    if kmeansplusplus:
        centroids = initialize_centroids_kmeans_pp(data, num_centroids)
    else:
        centroids = initialize_centroids_forgy(data, num_centroids)


    assignments  = assign_to_cluster(data, centroids)
    for i in range(100): # max number of iteratSion = 100
        print(f"Intra distance after {i} iterations: {mean_intra_distance(data, assignments, centroids)}")
        centroids = update_centroids(data, assignments)
        new_assignments = assign_to_cluster(data, centroids)
        if np.all(new_assignments == assignments): # stop if nothing changed
            break
        else:
            assignments = new_assignments

    return new_assignments, centroids, mean_intra_distance(data, new_assignments, centroids)

