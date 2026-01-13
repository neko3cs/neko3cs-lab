import * as THREE from "three";
import { OrbitControls } from "three/examples/jsm/controls/OrbitControls.js";

// シーン
const scene = new THREE.Scene();
scene.background = new THREE.Color(0x87ceeb); // 空色

// カメラ
const camera = new THREE.PerspectiveCamera(
  60,
  window.innerWidth / window.innerHeight,
  0.1,
  1000
);
camera.position.set(30, 40, 30);

// レンダラー
const renderer = new THREE.WebGLRenderer({ antialias: true });
renderer.setSize(window.innerWidth, window.innerHeight);
renderer.shadowMap.enabled = true;
document.body.appendChild(renderer.domElement);

// マウス操作
const controls = new OrbitControls(camera, renderer.domElement);
controls.enableDamping = true;

// ライト
const ambientLight = new THREE.AmbientLight(0xffffff, 0.4);
scene.add(ambientLight);

const dirLight = new THREE.DirectionalLight(0xffffff, 0.8);
dirLight.position.set(20, 40, 20);
dirLight.castShadow = true;
scene.add(dirLight);

// 地面
const groundGeometry = new THREE.PlaneGeometry(200, 200);
const groundMaterial = new THREE.MeshStandardMaterial({ color: 0x444444 });
const ground = new THREE.Mesh(groundGeometry, groundMaterial);
ground.rotation.x = -Math.PI / 2;
ground.receiveShadow = true;
scene.add(ground);

// ビル生成
const buildingGroup = new THREE.Group();
scene.add(buildingGroup);

const buildingGeometry = new THREE.BoxGeometry(1, 1, 1);

for (let x = -10; x <= 10; x += 2) {
  for (let z = -10; z <= 10; z += 2) {
    const height = Math.random() * 10 + 2;

    const material = new THREE.MeshStandardMaterial({
      color: new THREE.Color().setHSL(Math.random(), 0.5, 0.5),
    });

    const building = new THREE.Mesh(buildingGeometry, material);
    building.scale.set(1, height, 1);
    building.position.set(x, height / 2, z);
    building.castShadow = true;

    buildingGroup.add(building);
  }
}

// 自動回転用
let angle = 0;

// アニメーション
function animate() {
  requestAnimationFrame(animate);

  // 自動でくるくる
  angle += 0.002;
  const radius = 40;
  camera.position.x = Math.cos(angle) * radius;
  camera.position.z = Math.sin(angle) * radius;
  camera.lookAt(0, 0, 0);

  controls.update();
  renderer.render(scene, camera);
}

animate();

// リサイズ対応
window.addEventListener("resize", () => {
  camera.aspect = window.innerWidth / window.innerHeight;
  camera.updateProjectionMatrix();
  renderer.setSize(window.innerWidth, window.innerHeight);
});
