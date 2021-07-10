package audio

import "github.com/kkdai/youtube/v2"

type AudioLoader struct {
	client youtube.Client
}

func (l *AudioLoader) LoadSong(url string) string {
	video, err := l.client.GetVideo(url)
	if err != nil {
		return ""
	}

	streamUrl, err := l.client.GetStreamURL(video, &video.Formats.AudioChannels(2)[0])
	if err != nil {
		return ""
	}

	return streamUrl
}
