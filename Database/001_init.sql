CREATE TABLE IF NOT EXISTS songs
(
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(500) NOT NULL,
    video_url VARCHAR(500) NOT NULL,
    resource_url VARCHAR(500) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UNIQUE (name, video_url)
);

CREATE TABLE IF NOT EXISTS rounds    
(
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    total_matches INT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS matches
(
    id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    first_song_id INT NOT NULL,
    second_song_id INT NOT NULL,
    winner_song_id INT,
    round_id INT NOT NULL,
    match_order INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (first_song_id) REFERENCES songs(id),
    FOREIGN KEY (second_song_id) REFERENCES songs(id),
    FOREIGN KEY (winner_song_id) REFERENCES songs(id),
    FOREIGN KEY (round_id) REFERENCES rounds(id)
);