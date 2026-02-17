package org.example;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.lang.ClassNotFoundException;
import java.net.ServerSocket;
import java.net.Socket;

public class Server{

    private static ServerSocket server;
    private static int port = 2115;

    public static void main(String[] args) throws IOException, ClassNotFoundException{
        server = new ServerSocket(port);
        while(true){
            System.out.println("Waiting for the message");
            Socket socket = server.accept();
            ObjectInputStream ois = new ObjectInputStream(socket.getInputStream());
            String message = (String) ois.readObject();
            System.out.println("Message-in: " + message);
            ObjectOutputStream oos = new ObjectOutputStream(socket.getOutputStream());
            System.out.println("Message-out: " + message.toUpperCase());
            oos.writeObject("Message-out: " + message.toUpperCase());
            ois.close();
            oos.close();
            socket.close();
            if(message.equalsIgnoreCase("0")) break;
        }
        server.close();
    }

}