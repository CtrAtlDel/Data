import Model.Packet;
import Receiver.Receiver;
import Sender.Sender;
import IGotYouBabe.Song;

public class Solver {

    public static void main(String[] args) {
        Song song  = new Song();
        Packet pack = new Packet();
        Thread sender = new Thread(new Sender(pack));
        Thread receiver = new Thread(new Receiver(pack));

        sender.start();
        receiver.start();
    }
}
