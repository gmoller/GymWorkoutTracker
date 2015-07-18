DROP TABLE IF EXISTS exercise_instance;

DROP TABLE IF EXISTS exercise;

DROP TABLE IF EXISTS muscle;

DROP TABLE IF EXISTS muscle_group;

CREATE TABLE IF NOT EXISTS muscle_group
(
  id INT(11) NOT NULL AUTO_INCREMENT,
  name VARCHAR(50) NOT NULL,
  date_created TIMESTAMP DEFAULT NOW(),
  date_modified TIMESTAMP DEFAULT NOW(),
  CONSTRAINT muscle_group_pk PRIMARY KEY (id),
  CONSTRAINT muscle_group_name_uq UNIQUE (name)
);

CREATE TABLE IF NOT EXISTS muscle
(
  id INT(11) NOT NULL AUTO_INCREMENT,
  name VARCHAR(50) NOT NULL,
  muscle_group_id INT(11) NOT NULL,
  date_created TIMESTAMP DEFAULT NOW(),
  date_modified TIMESTAMP DEFAULT NOW(),
  CONSTRAINT muscle_pk PRIMARY KEY (id),
  INDEX muscle_group_id_ix (muscle_group_id),
  CONSTRAINT muscle_group_id_fk FOREIGN KEY muscle_group_id_ix (muscle_group_id) REFERENCES muscle_group (id),
  CONSTRAINT muscle_name_uq UNIQUE (name)
);

CREATE TABLE IF NOT EXISTS exercise
(
  id INT(11) NOT NULL AUTO_INCREMENT,
  exrx_name VARCHAR(50) DEFAULT NULL,
  alternate_name VARCHAR(50) NOT NULL,
  url VARCHAR(100) NOT NULL,
  muscle_id INT(11) NOT NULL,
  date_created TIMESTAMP DEFAULT NOW(),
  date_modified TIMESTAMP DEFAULT NOW(),
  CONSTRAINT exercise_pk PRIMARY KEY (id),
  INDEX muscle_id_ix (muscle_id),
  CONSTRAINT muscle_id_fk FOREIGN KEY muscle_id_ix (muscle_id) REFERENCES muscle (id),
  CONSTRAINT exercise_exrx_name_uq UNIQUE (exrx_name),
  CONSTRAINT exercise_alternate_name_uq UNIQUE (alternate_name)
);

CREATE TABLE IF NOT EXISTS exercise_instance
(
  id INT(11) NOT NULL AUTO_INCREMENT,
  date DATETIME NOT NULL,
  exercise_id INT(11) NOT NULL,
  set_number INT(11) NOT NULL,
  number_of_reps INT(11) NOT NULL,
  weight FLOAT(5,1) NOT NULL,
  date_created TIMESTAMP DEFAULT NOW(),
  date_modified TIMESTAMP DEFAULT NOW(),
  CONSTRAINT exercise_instance_pk PRIMARY KEY (id),
  INDEX exercise_id_ix (exercise_id),
  CONSTRAINT exercise_id_fk FOREIGN KEY exercise_id_ix (exercise_id) REFERENCES exercise (id),
  CONSTRAINT exercise_instance_date_uq UNIQUE (date)
);

INSERT INTO muscle_group (name) VALUES ('Chest');
INSERT INTO muscle_group (name) VALUES ('Back');
INSERT INTO muscle_group (name) VALUES ('Calves');
INSERT INTO muscle_group (name) VALUES ('Shoulders');
INSERT INTO muscle_group (name) VALUES ('Upper Arms');
INSERT INTO muscle_group (name) VALUES ('Thighs');

INSERT INTO muscle (name, muscle_group_id) VALUES ('Pectoralis Major, Sternal', 1);
INSERT INTO muscle (name, muscle_group_id) VALUES ('Pectoralis Major, Cavicular', 1);
INSERT INTO muscle (name, muscle_group_id) VALUES ('Erector Spinae', 2);
INSERT INTO muscle (name, muscle_group_id) VALUES ('Back, General', 2);
INSERT INTO muscle (name, muscle_group_id) VALUES ('Latissimus Dorsi', 2);
INSERT INTO muscle (name, muscle_group_id) VALUES ('Gastrocnemius', 3);
INSERT INTO muscle (name, muscle_group_id) VALUES ('Deltoid, Anterior', 4);
INSERT INTO muscle (name, muscle_group_id) VALUES ('Deltoid, Lateral', 4);
INSERT INTO muscle (name, muscle_group_id) VALUES ('Deltoid, Posterior', 4);
INSERT INTO muscle (name, muscle_group_id) VALUES ('Biceps Brachii', 5);
INSERT INTO muscle (name, muscle_group_id) VALUES ('Triceps Brachii', 5);
INSERT INTO muscle (name, muscle_group_id) VALUES ('Quadriceps', 6);
INSERT INTO muscle (name, muscle_group_id) VALUES ('Hamstrings', 6);

INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Barbell Bench Press', 'Barbell Flat Bench Press', 'http://www.exrx.net/WeightExercises/PectoralSternal/BBBenchPress.html', 1);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Barbell Incline Bench Press', 'Barbell Bench Press (incline)', 'http://www.exrx.net/WeightExercises/PectoralClavicular/BBInclineBenchPress.html', 2);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Weighted Chest Dip', 'Bar Dips, Bent Over', 'http://www.exrx.net/WeightExercises/PectoralSternal/WtChestDip.html', 1);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Barbell Deadlift', 'Barbell Deadlift', 'http://www.exrx.net/WeightExercises/ErectorSpinae/BBDeadlift.html', 3);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Dumbbell Bent-over Row', 'Dumbbell Bent-over Row', 'http://www.exrx.net/WeightExercises/BackGeneral/DBBentOverRow.html', 4);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Cable Close Grip Pulldown', 'Cable Close Grip Pulldown', 'http://www.exrx.net/WeightExercises/LatissimusDorsi/CBCloseGripPulldown.html', 5);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Lever Standing Calf Raise', 'Machine Calf Rocking (standing)', 'http://www.exrx.net/WeightExercises/Gastrocnemius/LVStandingCalfRaise.html', 6);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Barbell Military Press', 'Barbell Shoulder Press (standing)', 'http://www.exrx.net/WeightExercises/DeltoidAnterior/BBMilitaryPress.html', 7);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Dumbbell Lateral Raise', 'Dumbbell Lateral Raise (standing)', 'http://www.exrx.net/WeightExercises/DeltoidLateral/DBLateralRaise.html', 8);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Dumbbell Rear Lateral Raise', 'Dumbbell Lateral Raise (bent over)', 'http://www.exrx.net/WeightExercises/DeltoidPosterior/DBRearLateralRaise.html', 9);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Dumbbell Curl', 'Dumbbell Alternating Biceps Curl (standing)', 'http://www.exrx.net/WeightExercises/Biceps/DBCurl.html', 10);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Barbell Curl', 'Barbell Biceps Curl (standing)', 'http://www.exrx.net/WeightExercises/Biceps/BBCurl.html', 10);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Cable Pushdown', 'Pulley Machine Triceps Press (standing)', 'http://www.exrx.net/WeightExercises/Triceps/CBPushdown.html', 11);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Dumbbell Triceps Extension', 'Overhead Both Arm Dumbbell Triceps Press (seated)', 'http://www.exrx.net/WeightExercises/Triceps/DBTriExt.html', 11);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Barbell Squat', 'Barbell Squats (standing)', 'http://www.exrx.net/WeightExercises/Quadriceps/BBSquat.html', 12);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Sled 45 Leg Press', 'Leg Press Machine (incline)', 'http://www.exrx.net/WeightExercises/Quadriceps/SL45LegPress.html', 12);
INSERT INTO exercise (exrx_name, alternate_name, url, muscle_id) VALUES ('Barbell Straight Leg Deadlift', 'Barbell Romanian Deadlift', 'http://www.exrx.net/WeightExercises/Hamstrings/BBStraightLegDeadlift.html', 13);

COMMIT;
