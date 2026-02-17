package org.example;

import java.io.IOException;
import java.util.Scanner;
public class InputManager implements Runnable {
    private Server server;
    public InputManager(Server server){
        this.server=server;
    }
    public void run(){
        Scanner scan = new Scanner(System.in);
        String czyKoniec = scan.nextLine();
        try {
            this.server.stopServer();
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }
}
