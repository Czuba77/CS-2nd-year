from exceptions import GameplayException
from connect4 import Connect4
from randomagent import RandomAgent
from minmaxagent import MinMaxAgent
from alphabetaagent import AlphaBetaAgent

connect4 = Connect4(width=7, height=6)
#agent1 = RandomAgent('o')
agent1 = AlphaBetaAgent('o',False)
#agent2 = MinMaxAgent('x')
agent2 = AlphaBetaAgent('x')
x_wins=0
o_wins=0
for i in range(0,100):
    while not connect4.game_over:
        connect4.draw()
        try:
            if connect4.who_moves == agent1.my_token:
                n_column = agent1.decide(connect4,3)
            else:
                n_column = agent2.decide(connect4,3)
            connect4.drop_token(n_column)
        except (ValueError, GameplayException):
            print('invalid move')
    if connect4.getWins() == 'x':
        x_wins += 1
    else:
        o_wins += 1
    connect4.draw()
connect4.draw()
print(x_wins,o_wins)
