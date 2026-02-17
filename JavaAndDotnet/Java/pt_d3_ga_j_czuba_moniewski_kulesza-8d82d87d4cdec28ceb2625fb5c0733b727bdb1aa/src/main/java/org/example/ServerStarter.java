package org.example;

import java.io.IOException;

public class ServerStarter {
    public static void main(String[] args){
        try {
            Server server = new Server();
        } catch (IOException | ClassNotFoundException e) {
            throw new RuntimeException(e);
        }
    }
}
