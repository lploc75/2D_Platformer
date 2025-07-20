import os

folder = '.'  # Thư mục hiện tại, hoặc thay bằng đường dẫn đến folder bạn cần

for filename in os.listdir(folder):
    if filename.endswith('.yaml'):
        base = os.path.splitext(filename)[0]
        new_name = base + '.asset'
        os.rename(os.path.join(folder, filename), os.path.join(folder, new_name))
        print(f"Renamed: {filename} -> {new_name}")

print("Done!")
