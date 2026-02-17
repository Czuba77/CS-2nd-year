package org.example;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.InetAddress;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.Scanner;

public class Client {

    public static void main(String[] args) throws UnknownHostException, IOException, ClassNotFoundException, InterruptedException{
        //get the localhost IP address, if server is running on some other IP, you need to use that
        InetAddress host = InetAddress.getLocalHost();
        Scanner in = new Scanner(System.in);
        Socket socket = null;
        ObjectOutputStream oos = null;
        ObjectInputStream ois = null;
        while(true){
            socket = new Socket(host.getHostName(), 2115);
            oos = new ObjectOutputStream(socket.getOutputStream());
            //dodac scanner
            String messageSent = in.nextLine();
            oos.writeObject(messageSent);//gowno ze scannera
            if(messageSent.equals("0")){
                Thread.sleep(200);
                oos.close();
                ois.close();
                socket.close();
                break;
            }
            ois = new ObjectInputStream(socket.getInputStream());
            String messageRec = (String) ois.readObject();
            System.out.println("Message-received: " + messageRec);
            ois.close();
            oos.close();

            Thread.sleep(100);
        }
    }
}