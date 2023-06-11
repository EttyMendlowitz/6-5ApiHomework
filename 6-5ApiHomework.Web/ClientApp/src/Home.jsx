import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import useInterval from './useInterval';

const Home = () => {

    const [isLoading, setIsLoading] = useState(true);
    const [joke, setJoke] = useState({});
    const [interactionStatus, setInteractionStatus] = useState('');

    const updateCounts = async () => {
        if (!joke.id) {
            return;
        }
        const { data: likesCount } = await axios.get(`/api/jokes/getlikes?id=${joke.id}`);
        const { data: dislikesCount } = await axios.get(`/api/jokes/getdislikes?id=${joke.id}`);
        setJoke({ ...joke, likes: likesCount, dislikes: dislikesCount });
    }

    useInterval(updateCounts, 500);

    useEffect(() => {

        const getJoke = async () => {
            const { data } = await axios.get('/api/jokes/getjoke');
           const { data: interactionStatus } = await axios.get(`/api/jokes/getinteractionstatus/${data.id}`);
            setJoke(data);
            setInteractionStatus(interactionStatus.status);
            setIsLoading(false);

        }
        getJoke();

    }, []);

    const interactWithJoke = async(like) => {
        await axios.post('/api/jokes/interactwithjoke', { jokeId: joke.id, like });
        const { data: interactionStatus } = await axios.get(`/api/jokes/getinteractionstatus/${joke.id}`);
        setInteractionStatus(interactionStatus.status);
    }

    const canLike = interactionStatus !== 'Liked';
    const canDislike = interactionStatus !== 'Disliked';

    return (isLoading ? <h1>Loading...</h1> : <div className="container" style={{ marginTop: 60 }}>
        <div
            className="row"
            style={{ minHeight: "80vh", display: "flex", alignItems: "center" }}
        >
            <div className="col-md-6 offset-md-3 bg-light p-4 rounded shadow">
                <div>
                    <h4>
                        {joke.setup}
                    </h4>
                    <h4>{joke.punchline}</h4>
                    <div>
                        {interactionStatus !== 'Unauthenticated' && <div>
                            <button disabled={!canLike} onClick={() => interactWithJoke(true)}
                                className="btn btn-primary">Like
                            </button>
                            <button disabled={!canDislike} onClick={() => interactWithJoke(false)}
                                className="btn btn-danger">Dislike
                            </button>
                        </div>}
                        {interactionStatus === 'Unauthenticated' && <div>
                            <Link to='/login'>Login to your account to like/dislike this joke</Link>
                        </div>}

                        <h4>Likes: {joke.likes}  </h4>
                        <h4>Dislikes: {joke.dislikes} </h4>
                        <h4>
                            <button className="btn btn-link" onClick={() => window.location.reload()}>Refresh</button>
                        </h4>
                    </div>
                </div>
            </div>
        </div>
    </div>
    )
}

export default Home;