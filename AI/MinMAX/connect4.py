from exceptions import GameplayException


class Connect4:
    def __init__(self, width=5, height=4):
        self.width = width
        self.height = height
        self.who_moves = 'o'
        self.game_over = False
        self.wins = None
        self.board = []
        for n_row in range(self.height):
            self.board.append(['_' for _ in range(self.width)])

    def possible_drops(self):
        return [n_column for n_column in range(self.width) if self.board[0][n_column] == '_']

    def drop_token(self, n_column):
        if self.game_over:
            raise GameplayException('game over')
        if n_column not in self.possible_drops():
            raise GameplayException('invalid move')

        n_row = 0
        while n_row + 1 < self.height and self.board[n_row+1][n_column] == '_':
            n_row += 1
        self.board[n_row][n_column] = self.who_moves
        self.game_over = self._check_game_over()
        self.who_moves = 'o' if self.who_moves == 'x' else 'x'

    def drop_test_token(self,n_column,token):
        n_row = 0
        while n_row + 1 < self.height and self.board[n_row+1][n_column] == '_':
            n_row += 1
        self.board[n_row][n_column] = token

    def remove_token(self,n_column):
        n_row = self.height
        while n_row - 1 >= 0 and self.board[n_row - 1][n_column] != '_':
            n_row -= 1
        self.board[n_row][n_column] = '_'

    def center_column(self):
        return [self.board[n_row][self.width//2] for n_row in range(self.height)]

    def iter_fours(self):
        # horizontal
        for n_row in range(self.height):
            for start_column in range(self.width-3):
                yield self.board[n_row][start_column:start_column+4]

        # vertical
        for n_column in range(self.width):
            for start_row in range(self.height-3):
                yield [self.board[n_row][n_column] for n_row in range(start_row, start_row+4)]

        # diagonal
        for n_row in range(self.height-3):
            for n_column in range(self.width-3):
                yield [self.board[n_row+i][n_column+i] for i in range(4)]  # decreasing
                yield [self.board[n_row+i][self.width-1-n_column-i] for i in range(4)]  # increasing

    def iter_threes(self, movex,movey,token):
        # Sprawdzenie poziome w prawo
        if movex + 2 <= self.width-1 and self.board[movey][movex + 1] == token and self.board[movey][
            movex + 2] == token:
            return True

        # Sprawdzenie poziome w lewo
        if movex - 2 >= 0 and self.board[movey][movex - 1] == token and self.board[movey][movex - 2] == token:
            return True

        # Sprawdzenie pionowe w dół
        if movey + 2 <= self.height-1 and self.board[movey + 1][movex] == token and self.board[movey + 2][
            movex] == token:
            return True

        # Sprawdzenie pionowe w górę
        if movey - 2 >= 0 and self.board[movey - 1][movex] == token and self.board[movey - 2][movex] == token:
            return True

        # Sprawdzenie diagonalne (prawy dolny)
        if movex + 2 <= self.width-1 and movey + 2 <= self.height-1 and self.board[movey + 1][movex + 1] == token and \
                self.board[movey + 2][movex + 2] == token:
            return True

        # Sprawdzenie diagonalne (lewy dolny)
        if movex - 2 >= 0 and movey + 2 <= self.height-1 and self.board[movey + 1][movex - 1] == token and \
                self.board[movey + 2][movex - 2] == token:
            return True

        # Sprawdzenie diagonalne (prawy górny)
        if movex + 2 <= self.width-1 and movey - 2 >= 0 and self.board[movey - 1][movex + 1] == token and \
                self.board[movey - 2][movex + 2] == token:
            return True

        # Sprawdzenie diagonalne (lewy górny)
        if movex - 2 >= 0 and movey - 2 >= 0 and self.board[movey - 1][movex - 1] == token and \
                self.board[movey - 2][movex - 2] == token:
            return True

        return False

    def check_threes(self, moves,token):
        for i in range(0,len(moves)):
            if self.iter_threes(moves[i][0],moves[i][1],token):
                return True
        return False

    def _check_game_over(self):
        if not self.possible_drops():
            self.wins = None  # tie
            return True

        for four in self.iter_fours():
            if four == ['o', 'o', 'o', 'o']:
                self.wins = 'o'
                return True
            elif four == ['x', 'x', 'x', 'x']:
                self.wins = 'x'
                return True
        return False

    def getWins(self):
        return self.wins

    def draw(self):
        for row in self.board:
            print(' '.join(row))
        if self.game_over:
            print('game over')
            print('wins:', self.wins)
        else:
            print('now moves:', self.who_moves)
            print('possible drops:', self.possible_drops())


    def getWidth(self):
        return self.width

    def setWho_moves(self,who_moves):
        self.who_moves=who_moves

    def getYofMove(self,move,token):
        for i in range(0,len(self.board)):
            if self.board[i][move]==token:
                return i

    def getTokenFromBoard(self, x,y):
        return self.board[x][y]