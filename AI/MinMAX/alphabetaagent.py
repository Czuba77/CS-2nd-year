from exceptions import AgentException



class AlphaBetaAgent:


    def __init__(self, my_token='x', isHeuristickOn=True):
        self.my_token = my_token
        if my_token == 'x':
            self.enemy_token = 'o'
        else:
            self.enemy_token = 'x'
        self.isHeuristickOn = isHeuristickOn

    def decide(self, connect4, d):
        if connect4.who_moves != self.my_token:
            raise AgentException('not my round')
        connect4tmp = connect4
        alfa=-10.0
        beta=10.0
        moves=[]
        moveIndex, rating = self.maxLocaal(connect4tmp, d,alfa,beta,moves)
        connect4.setWho_moves(self.my_token)
        return moveIndex

    def MinMaxRating(self, connect4, d, x,alfa,beta,moves):
        if connect4._check_game_over():
            if connect4.getWins() == self.my_token:
                return 1.0
            elif connect4.getWins() != self.my_token and connect4.getWins() is not None:
                return -1.0
            else:
                return 0.0
        elif d == 0 and self.isHeuristickOn:
            var = self.heuristicStateRating(connect4,moves,x)
            return var
        elif d == 0:
            return 0.0
        elif x == 1:
            moveIndex, rating = self.minLocaal(connect4, d - 1,alfa,beta,moves)
            return rating
        else:
            moveIndex, rating = self.maxLocaal(connect4, d - 1,alfa,beta,moves)
            return rating

    def isOnMiddle(self,connect4,moves,x):
        if x==1:
            token=self.my_token
        else:
            token=self.enemy_token
        for i in range(0, len(moves)):
            var = int(connect4.getWidth()/2)
            if (moves[i][1] == var and token==connect4.getTokenFromBoard(moves[i][1],moves[i][0])) or (moves[i][1] == var+1 and token==connect4.getTokenFromBoard(moves[i][1],moves[i][0])):
                return True
        return False

    def heuristicStateRating(self, connect4, moves,x):
        sum=0.0
        if connect4.check_threes(moves,self.my_token):
            sum+= 0.50
        if self.isOnMiddle(connect4, moves,x):
            sum+= 0.20
        if x==1:
            return sum
        else:
            return -sum

    def maxLocaal(self, connect4, d,alfa,beta,moves_og):
        mValue = []
        for i in range(0, connect4.getWidth()):
            mValue.append(-100.0)
        v=-10.0
        index = 0
        for move in connect4.possible_drops():
            connect4.drop_test_token(move, self.my_token)
            moves=[]
            for mo in moves_og:
                moves.append(mo)
            moves.append((move,connect4.getYofMove(move,self.my_token)))
            mValue[move] = (self.MinMaxRating(connect4, d, 1,alfa,beta,moves))
            connect4.remove_token(move)
            if mValue[move]>v:
                v=mValue[move]
                index=move
            alfa=max(alfa,v)
            if alfa==1.0:
                break
        return index, v

    def minLocaal(self, connect4, d,alfa,beta,moves_og):
        mValue = []
        v=10.0
        index = 0
        for i in range(0, connect4.getWidth()):
            mValue.append(100.0)
        for move in connect4.possible_drops():
            moves=moves_og
            connect4.drop_test_token(move, self.enemy_token)
            moves.append((move, connect4.getYofMove(move, self.enemy_token)))
            mValue[move] = (self.MinMaxRating(connect4, d, 0,alfa,beta,moves))
            connect4.remove_token(move)
            if mValue[move]<v:
                v=mValue[move]
                index=move
            beta=min(beta,v)
            if beta==-1.0:
                break
        return index, v