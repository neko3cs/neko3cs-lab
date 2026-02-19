-- テーブルの作成
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    full_name VARCHAR(100),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- テストデータの投入
INSERT INTO users (username, email, full_name) VALUES
('tanaka', 'tanaka@example.com', 'Taro Tanaka'),
('suzuki', 'suzuki@example.com', 'Ichiro Suzuki'),
('sato', 'sato@example.com', 'Hanako Sato'),
('ito', 'ito@example.com', 'Ken Ito'),
('watanabe', 'watanabe@example.com', 'Yuki Watanabe')
ON CONFLICT (username) DO NOTHING;
