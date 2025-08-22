
// Vanilla JS (in <script> am Ende des HTML)
// document.body.addEventListener("mousemove", (e) => {
//   const tiltX = (e.clientY / window.innerHeight) * 20 - 10;
//   const tiltY = (e.clientX / window.innerWidth) * -20 + 10;
//   document.body.style.transform = `rotateX(${tiltX}deg) rotateY(${tiltY}deg)`;
// });

// NEUE ZEILE
import * as THREE from 'https://cdn.jsdelivr.net/npm/three@0.179.1/build/three.module.js';

// 1.SZENE 
// Kamera und Renderer 
const scene = new THREE.Scene();
const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
const renderer = new THREE.WebGLRenderer({
  canvas: document.querySelector('#bg'),
});

renderer.setPixelRatio(window.devicePixelRatio);
renderer.setSize(window.innerWidth, window.innerHeight);
camera.position.setZ(30);

// 2. Hintergrundbild laden
const textureLoader = new THREE.TextureLoader();
textureLoader.load('/assets/images/background.jpg', function(texture) {
  scene.background = texture;
});

// 3. Partikel (Sterne) erstellen
const particleGeometry = new THREE.BufferGeometry();
const particleCount = 5000; // Anzahl der Sterne

const positions = new Float32Array(particleCount * 3); // Jedes Partikel hat 3 Werte (x, y, z)

for (let i = 0; i < particleCount * 3; i++) {
  // Zufällige Position im Raum
  positions[i] = (Math.random() - 0.5) * 500; 
}

particleGeometry.setAttribute('position', new THREE.BufferAttribute(positions, 3));


const particleMaterial = new THREE.PointsMaterial({
  size: 0.05,
  color: 0xffffff,
});


const particles = new THREE.Points(particleGeometry, particleMaterial);
scene.add(particles);

// 4. Lichter (optional, aber gut für zukünftige 3D-Modelle)
const pointLight = new THREE.PointLight(0xffffff);
pointLight.position.set(5, 5, 5);
scene.add(pointLight);

const ambientLight = new THREE.AmbientLight(0xffffff, 0.2);
scene.add(ambientLight);


// 5. Maus-Interaktion für Parallax-Effekt
let mouseX = 0;
let mouseY = 0;

document.addEventListener('mousemove', (event) => {
  mouseX = event.clientX - (window.innerWidth / 2);
  mouseY = event.clientY - (window.innerHeight / 2);
});


// 6. Animations-Loop
function animate() {
  requestAnimationFrame(animate);

  // Langsame Rotation der Sterne für "lebendiges" Gefühl
  particles.rotation.x += 0.0001;
  particles.rotation.y += 0.0001;
  
  // Kamera bewegt sich basierend auf der Mausposition
  camera.position.x += (mouseX - camera.position.x) * 0.0005;
  camera.position.y += (-mouseY - camera.position.y) * 0.0005;
  camera.lookAt(scene.position);

  renderer.render(scene, camera);
}

// 7. Fenstergröße anpassen
window.addEventListener('resize', () => {
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(window.innerWidth, window.innerHeight);
});

animate();
