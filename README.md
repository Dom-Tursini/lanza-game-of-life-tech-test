# Conway's Game of Life – Tech Demo

## 🧠 Overview

This tech demo is an implementation of **Conway's Game of Life** using **Angular** for the frontend and **C# (.NET)** for the backend. The goal was to create a fully functional, visually appealing, and interactive simulation of the classic cellular automaton.

The app includes:

- ✅ Start / Stop / Reset controls  
- ✅ Grid size selection  
- ✅ Seed pattern selection  
- ✅ Real-time updates via SignalR  
- ✅ Unit-tested backend logic  

Additional enhancements include **custom branding** and **visual flair** through animated sprites.

---

## ⚙️ Challenges and Solutions

### 🔄 Neighbor State Calculation

A fundamental challenge was ensuring that neighbor state calculations were based on the **previous generation** — not the current one in-progress. Two approaches were evaluated:

- **New Grid per Generation:** Create a new array each tick and calculate next-gen values without modifying the source.
- **Double Buffering:** Alternate between two arrays, using one for reading and one for writing.

🔧 **Solution Used:** New array per generation — prioritized for clarity and code simplicity.

---

### 🌐 Edge Cell Neighbor Handling

Edge behavior is often a sticking point. Options explored:

- Manually handle corner and edge logic ❌
- Treat edges as always-dead or always-alive ❌
- **Toroidal wrapping using modulus** ✅

Wrapping rows/columns with modular arithmetic effectively turns the grid into a **donut-shaped surface**, giving all cells 8 neighbors consistently.

---

## 🖥️ Backend and Real-Time Communication

The backend is powered by **.NET + SignalR**, allowing real-time WebSocket-style communication with the Angular frontend.

### 🧪 Unit Testing (`GameOfLifeServiceTests.cs`)

Tests cover:

- Grid initialization and seeding  
- Start/Stop/Step logic  
- Accurate generation updates  
- Grid reset  
- SignalR broadcast mocking (mock clients injected)  

Advanced C# techniques like **reflection** and **interface mocking** were used to test internal logic and side effects without requiring live sockets.

---

## 🎭 Visual Enhancements and Easter Egg

This project includes custom sprites inspired by the **"Playtest" episode of Black Mirror**, nicknamed **Thronglets**.

- 🟢 **Alive cells:** Full sprite  
- 🟡 **Dying cells:** Transitional state (1 gen only)  
- ⚫ **Dead cells:** Transparent (not rendered)

The dying state is **visual only** and doesn’t affect logic. This improves clarity and gives the grid a smoother, animated feel.

---

## 🎨 Additional Features

- 📐 **Dynamic Grid Size:** Users can scale the grid on demand
- 🌱 **Seed Pattern Picker:** Predefined patterns like Glider, Pulsar, Angel
- 🎯 **Custom Branding:** Logos, titles, and dark UI theme

---

## ✅ Conclusion

This project successfully captures the spirit and logic of **Conway’s Game of Life**, solving several implementation challenges while adding unique touches in both code and presentation.

It stands as a technically complete and visually polished demo with:

- Reliable game logic  
- Real-time frontend/backend integration  
- Comprehensive unit testing  
- User-focused interaction features  
