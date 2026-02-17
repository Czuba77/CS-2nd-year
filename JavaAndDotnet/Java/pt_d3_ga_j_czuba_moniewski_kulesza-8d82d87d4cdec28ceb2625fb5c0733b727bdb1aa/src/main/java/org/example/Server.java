package org.example;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

public class Server {
    private DateTimeFormatter TimeFormatter =  DateTimeFormatter.ofPattern("HH:mm:ss");
    private ServerSocket server;
    private int port = 9876;
    private Runnable runnStop;
    private Thread threadStop;
    private List<Runnable> Runnables = new ArrayList<Runnable>();
    private List<Thread> Threads = new ArrayList<Thread>();
    Socket ClientSocket;

    public void stopServer() throws IOException{
        System.out.println("Shutting down Socket server!");
        for (Thread t : Threads) {
            t.stop();
        }
        server.close();
    }

    public Server() throws IOException, ClassNotFoundException{
        server = new ServerSocket(port);
        runnStop= new InputManager(this);
        threadStop = new Thread(runnStop);
        threadStop.start();
        while(true){
            LocalTime Time = LocalTime.now();
            System.out.println("| " + Time.format(TimeFormatter) + " | > Waiting for the client request...");
            try {
                ClientSocket = server.accept();
            }
            catch (IOException e) {
                break;
            }
            String handlerID = UUID.randomUUID().toString();
            Time = LocalTime.now();
            System.out.println("| " + Time.format(TimeFormatter) + " | > Client request accepted, client handler ID:" + handlerID);
            ServerThread newHandler = new ServerThread(ClientSocket, this, handlerID, TimeFormatter);
            Thread newthread = new Thread(newHandler);
            newthread.start();
            this.Runnables.add(newHandler);
            this.Threads.add(newthread);
            int tempSize = Threads.size();
            for(int i=0;i<tempSize;i++){
                if(!Threads.get(i).isAlive()){
                    Threads.remove(i);
                    Runnables.remove(i);
                    i--;
                    tempSize--;
                }
            }
            Time = LocalTime.now();
            System.out.println("| " + Time.format(TimeFormatter) + " | > Threads: " + Threads.size());
        }

    }

}
