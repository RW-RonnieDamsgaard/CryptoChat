# CryptoChat App
A simple Blazor + SignalR chat application with end-to-end encryption.

## How to Run
1. Clone the repository: `git clone <repo-url>`
2. Navigate to the project folder: `cd CryptoChat`
3. Restore dependencies: `dotnet restore`
4. Run the app: `dotnet run`
5. Open two browser windows to `https://localhost:5001/chat` and chat securely!

## Cryptography
- **Confidentiality**: Messages are encrypted with AES-256 using a pre-shared key. The key is hashed to 32 bytes, and the first 16 bytes are used as the IV.
- **Integrity**: Each message includes an HMAC-SHA256 signature, verified on receipt to ensure no tampering.
- **Why**: AES provides strong encryption, and HMAC ensures message integrity. A pre-shared key simplifies this POC, though a real app would use key exchange (e.g., Diffie-Hellman).

## Screenshots
![image](https://github.com/user-attachments/assets/8de43fd3-8d17-4795-aaa5-dfa15314f21b)


## Notes
- Both users must use the same key (hardcoded for simplicity).
- This is a proof-of-concept, not production-ready.
