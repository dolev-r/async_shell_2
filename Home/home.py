import socket

buffer_size = 100

def main():
    print("Server started")
    HOST = ''                 # Symbolic name meaning all available interfaces
    PORT = 12345              # Arbitrary non-privileged port
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.bind((HOST, PORT))
        s.listen(1)
        while True:
            conn, addr = s.accept()
            with conn:
                print('Connected by', addr)
                while True:
                    data = conn.recv(buffer_size)
                    print(data)
                    if not data: break
                    x = b"hello alan"
                    conn.send(len(x).to_bytes(4, byteorder='little'))
                    conn.send(x)



if __name__ == "__main__":
    main()