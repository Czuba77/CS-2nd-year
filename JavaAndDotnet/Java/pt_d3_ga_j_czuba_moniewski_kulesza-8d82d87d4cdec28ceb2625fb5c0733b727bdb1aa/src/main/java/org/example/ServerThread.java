package org.example;
import java.io.ObjectInputStream;
import java.net.Socket;
import java.io.IOException;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;

public class ServerThread implements Runnable {
    private Socket clientSocket;
    private Server parent;
    private ObjectInputStream input;
    private String ID;
    private DateTimeFormatter TimeFormatter;
;
    public ServerThread(Socket clientSocket, Server parent, String HandlerID, DateTimeFormatter formatter) throws IOException{
        this.clientSocket = clientSocket;
        this.parent = parent;
        this.input = new ObjectInputStream(clientSocket.getInputStream());
        this.ID = HandlerID;
        this.TimeFormatter = formatter;
    }
    public void run() {
        try {
            String message = (String) input.readObject();
            LocalTime Time = LocalTime.now();
            System.out.println("| " + Time.format(TimeFormatter) + " | > Message from server Received: " + message);
            System.out.println("| " + Time.format(TimeFormatter) + " | > Closing the thread " + ID);
            this.input.close();
            this.clientSocket.close();

        } catch (IOException | ClassNotFoundException e) {
            throw new RuntimeException(e);
        }
    }
}
