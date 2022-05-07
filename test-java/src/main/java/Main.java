import IGotYouBabe.Song;

public class Main {
    public static void main(String[] args) {
        Song song = new Song();
        String[][] lyrics = song.lyrics;
        for (int i = 0; i < lyrics.length; i++) {

            System.out.println("" + lyrics[i][0]);
        }

    }
}
