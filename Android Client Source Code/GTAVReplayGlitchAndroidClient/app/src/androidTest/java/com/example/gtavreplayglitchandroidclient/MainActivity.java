package com.example.gtavreplayglitchandroidclient;

import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import java.io.OutputStream;
import java.net.InetSocketAddress;
import java.net.Socket;

class MainActivity extends AppCompatActivity {

    private EditText ipField, portField;
    private Button connectButton, replayGlitchButton;
    private TextView statusLabel;

    private Socket socket;
    private Handler handler = new Handler();
    private boolean isConnected = false;

    private static final String PREFS_NAME = "ClientAppPrefs";
    private static final String PREF_IP = "ip";
    private static final String PREF_PORT = "port";
    private static final String KEEP_ALIVE_HEADER = "KEEP_ALIVE";
    private static final String REPLAY_GLITCH_HEADER = "REPLAY_GLITCH";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        ipField = findViewById(R.id.ipField);
        portField = findViewById(R.id.portField);
        connectButton = findViewById(R.id.connectButton);
        replayGlitchButton = findViewById(R.id.replayGlitchButton);
        statusLabel = findViewById(R.id.statusLabel);

        // Load saved IP and Port
        SharedPreferences prefs = getSharedPreferences(PREFS_NAME, MODE_PRIVATE);
        ipField.setText(prefs.getString(PREF_IP, ""));
        portField.setText(prefs.getString(PREF_PORT, ""));

        connectButton.setOnClickListener(v -> connectToServer());
        replayGlitchButton.setOnClickListener(v -> sendReplayGlitchPacket());

        // Periodically send a keep-alive packet
        handler.postDelayed(this::sendKeepAlivePacket, 3000);
    }

    private void connectToServer() {
        String ip = ipField.getText().toString();
        String portStr = portField.getText().toString();

        if (ip.isEmpty() || portStr.isEmpty()) {
            statusLabel.setText("IP/Port Required");
            statusLabel.setTextColor(Color.RED);
            return;
        }

        int port = Integer.parseInt(portStr);

        // Save IP and Port
        SharedPreferences.Editor editor = getSharedPreferences(PREFS_NAME, MODE_PRIVATE).edit();
        editor.putString(PREF_IP, ip);
        editor.putString(PREF_PORT, portStr);
        editor.apply();

        new Thread(() -> {
            try {
                socket = new Socket();
                socket.connect(new InetSocketAddress(ip, port), 3000);

                runOnUiThread(() -> {
                    isConnected = true;
                    statusLabel.setText("Connected");
                    statusLabel.setTextColor(Color.GREEN);
                });
            } catch (Exception e) {
                Log.e("ClientApp", "Connection Failed", e);
                runOnUiThread(() -> {
                    isConnected = false;
                    statusLabel.setText("Disconnected");
                    statusLabel.setTextColor(Color.RED);
                });
            }
        }).start();
    }

    private void sendKeepAlivePacket() {
        if (isConnected && socket != null && socket.isConnected()) {
            sendPacket(KEEP_ALIVE_HEADER);
        }

        handler.postDelayed(this::sendKeepAlivePacket, 3000);
    }

    private void sendReplayGlitchPacket() {
        sendPacket(REPLAY_GLITCH_HEADER);
    }

    private void sendPacket(String header) {
        new Thread(() -> {
            try {
                if (socket != null && socket.isConnected()) {
                    OutputStream outputStream = socket.getOutputStream();
                    outputStream.write(header.getBytes());
                    outputStream.flush();
                }
            } catch (Exception e) {
                Log.e("ClientApp", "Packet Send Failed", e);
            }
        }).start();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        try {
            if (socket != null) {
                socket.close();
            }
        } catch (Exception e) {
            Log.e("ClientApp", "Socket Close Failed", e);
        }
    }
}
