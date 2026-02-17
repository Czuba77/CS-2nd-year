from exceptions import AgentException



class MinMaxAgent:
    def __init__(self, my_token='x',isHeuristickOn=True):
        self.my_token = my_token
        if my_token == 'x':
            self.enemy_token='o'
        else:
            self.enemy_token='x'
        self.isHeuristickOn=isHeuristickOn

    def decide(self, connect4,d):
        if connect4.who_moves != self.my_token:
            raise AgentException('not my round')
        connect4tmp=connect4
        moveIndex,rating=self.max(connect4tmp, d)
        return moveIndex

    def MinMaxRating(self, connect4,d,x):
        if connect4._check_game_over():
            if connect4.getWins()==self.my_token:
                return 1.0
            elif connect4.getWins()!=self.my_token and connect4.getWins() is not None:
                return -1.0
            else:
                return 0.0
        elif d == 0 and self.isHeuristickOn:
            return self.heuristicStateRating(connect4)
        elif x==1:
            moveIndex, rating = self.min(connect4, d - 1)
            return rating
        else:
            moveIndex, rating = self.max(connect4, d - 1)
            return rating


    def heuristicStateRating(self,connect4):
        return 0.0



    def max(self, connect4,d):
        mValue=[]
        for i in range(0,connect4.getWidth()):
            mValue.append(-10.0)

        for move in connect4.possible_drops():
            connect4.drop_test_token(move,self.my_token)
            mValue[move]=(self.MinMaxRating(connect4, d,1))
            connect4.remove_token(move)
        biggestmValue = -2.0
        index = 0
        for i in range(0, len(mValue)):
            if mValue[i] > biggestmValue and mValue[i]!=-10.0:
                index = i
                biggestmValue = mValue[i]
        return index,biggestmValue

    def min(self, connect4,d):
        mValue = []
        for i in range(0,connect4.getWidth()):
            mValue.append(10.0)
        for move in connect4.possible_drops():
            connect4.drop_test_token(move,self.enemy_token)
            mValue[move] = (self.MinMaxRating(connect4, d, 0))
            connect4.remove_token(move)
        smallestmValue = 2.0
        index = 0
        for i in range(0, len(mValue)):
            if mValue[i] < smallestmValue and mValue[i]!=10.0:
                index = i
                smallestmValue = mValue[i]
        return index,smallestmValue