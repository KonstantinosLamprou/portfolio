
// Vanilla JS (in <script> am Ende des HTML)
document.body.addEventListener("mousemove", (e) => {
  const tiltX = (e.clientY / window.innerHeight) * 20 - 10;
  const tiltY = (e.clientX / window.innerWidth) * -20 + 10;
  document.body.style.transform = `rotateX(${tiltX}deg) rotateY(${tiltY}deg)`;
});